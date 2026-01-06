using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics;

namespace Phaneritic.Implementations.Ledgering;

public class LedgerScribbler(
    ILedgeringContext ledgeringContext,
    IWorkCommitter workCommitter,
    IProvideAccessSession provideAccessSession,
    IOperationReader operationReader,
    IEnumerable<IManageOperation> manageOperations,
    ILudDictionary<ActivityTypeKey, ActivityTypeDto> activityTypes,
    IPackRecord<Models.Ledgering.Activity, ActivityDto> activityPacker
    ) : ILedgerScribbler
{
    // TODO: rate logging

    private readonly List<IManageOperation> ManageOperations = [.. manageOperations];
    private Stopwatch? _Timer;
    private Models.Ledgering.Activity? _Activity;
    private LedgerScribbler? _Uplink;
    private int _EntryIndex = 0;        // must yield to outer counter if _Yielded

    /// <summary>Gets next _EntryIndex from self or outer</summary>
    private int GetNextEntryIndex()
        => HasYieldedToOuter
        ? _Uplink!.GetNextEntryIndex()  // use outer entry numbering
        : ++_EntryIndex;                // preincrement and return

    public ActivityDto? ActiveActivity => activityPacker.Pack(_Activity);

    public bool HasYieldedToOuter => _Uplink != null;

    public void BeginActivity(ActivityTypeKey activityTypeKey, MethodKey methodKey, bool forceRestart = false)
    {
        if (HasYieldedToOuter)
        {
            return;
        }

        var _op = operationReader.GetSessionOperations().FirstOrDefault(_o => _o.MethodKey == methodKey)
            ?? ManageOperations.StartOperation(methodKey)
            ?? throw new InvalidOperationException(@"cannot start operation");

        // restart if activity type or operation id changes, or if force restart
        if ((_Activity != null)
            && ((_Activity.ActivityTypeKey != activityTypeKey)
            || (_Activity.OperationID != _op.OperationID)
            || forceRestart))
        {
            CompleteActivity();
        }

        // if no current activity, good to go
        if (_Activity == null)
        {
            _Timer = Stopwatch.StartNew();
            var _now = DateTimeOffset.Now;
            var _atKey = activityTypes.Get(activityTypeKey) is ActivityTypeDto _actType
                ? _actType.ActivityTypeKey
                : ActivityTypeKey.Undefined;
            var _session = provideAccessSession.CurrentAccessSession;

            _EntryIndex = 0;
            _Activity = new()
            {
                ActivityTypeKey = activityTypeKey,
                OperationID = _op.OperationID,
                MethodKey = methodKey,
                AccessMechanismID = _session?.AccessMechanism?.AccessMechanismID ?? default,
                AccessorID = _session?.Accessor?.AccessorID ?? default,
                AccessSessionID = _session?.AccessSessionID ?? default,
                StartAt = _now,
                InfoEntries = [],
                InventoryEntries = [],
                DemandEntries = [],
                FulfillmentEntries = [],
                ExceptionEntries = [],
            };
        }
    }

    public void ClearActivity()
    {
        if (!HasYieldedToOuter)
        {
            _Timer?.Stop();
            _Activity = null;
        }
    }

    public void CompleteActivity(bool isSuccessful = true)
    {
        if (!HasYieldedToOuter && (_Activity != null))
        {
            // gets ActivityID
            ledgeringContext.Activities.Add(_Activity);

            // summarize
            var _now = DateTimeOffset.Now;
            _Activity.IsSuccessful = isSuccessful;
            _Activity.EndAt = _now;
            _Activity.EntryCount = _EntryIndex;
            _Activity.InfoEntryCount = _Activity.InfoEntries!.Count;
            _Activity.InventoryEntryCount = _Activity.InventoryEntries!.Count;
            _Activity.DemandEntryCount = _Activity.DemandEntries!.Count;
            _Activity.FulfillmentEntryCount = _Activity.FulfillmentEntries!.Count;
            _Activity.ExceptionEntryCount = _Activity.ExceptionEntries!.Count;

            // timer stuff
            _Timer!.Stop();
            _Activity.DurationMilliSeconds = _Timer.ElapsedMilliseconds;
            _Activity.DurationMicroSeconds = (long)_Timer.Elapsed.TotalMicroseconds;

            workCommitter.CommitWork(ledgeringContext);

            // done
            _Activity = null;
        }
    }

    public void YieldToOuterActivity(ILedgerScribbler ledgerScribbler)
    {
        // same class, can inspect private fields
        if (ledgerScribbler is LedgerScribbler _private)
        {
            if (_Activity != null)
            {
                CompleteActivity();
            }
            _Activity = _private._Activity;
            _Timer = _private._Timer;
            _Uplink = _private;
        }
    }

    private TEntry NewEntry<TEntry>(int entryIndex, DateTimeOffset now, long milliOffset, long microOffset)
        where TEntry : CommonLedgerEntry, new()
        => new()
        {
            Activity = _Activity,
            EntryIndex = entryIndex,
            AccessorID = _Activity!.AccessorID,
            AccessMechanismID = _Activity!.AccessMechanismID,
            AccessSessionID = _Activity!.AccessSessionID,
            ActivityTypeKey = _Activity!.ActivityTypeKey,
            MethodKey = _Activity!.MethodKey,
            OperationID = _Activity!.OperationID,
            RecordedAt = now,
            OffsetMicroSeconds = microOffset,
            OffsetMilliSeconds = milliOffset
        };

    public void AddInfo(params List<InfoEntryParam> informations)
    {
        if (_Activity == null)
        {
            throw new InvalidOperationException(@"no current activity");
        }

        var _now = DateTimeOffset.Now;
        var _milli = _Timer!.ElapsedMilliseconds;
        var _micro = (long)_Timer!.Elapsed.TotalMicroseconds;
        var _index = GetNextEntryIndex();
        foreach (var _info in informations)
        {
            var _entry = NewEntry<InfoEntry>(_index, _now, _milli, _micro);
            _entry.InfoEntryKey = _info.InfoEntryKey;
            _entry.InfoEntryValue = _info.InfoEntryValue;
            _Activity.InfoEntries!.Add(_entry);
        }
    }

    public void AddInventory(params List<InventoryEntryParam> inventories)
    {
        if (_Activity == null)
        {
            throw new InvalidOperationException(@"no current activity");
        }

        var _now = DateTimeOffset.Now;
        var _milli = _Timer!.ElapsedMilliseconds;
        var _micro = (long)_Timer!.Elapsed.TotalMicroseconds;
        var _index = GetNextEntryIndex();
        foreach (var _info in inventories)
        {
            var _entry = NewEntry<InventoryEntry>(_index, _now, _milli, _micro);
            _entry.EntryLine = _info.EntryLine;
            _entry.ZoneID = _info.ZoneID;
            _entry.PositionID = _info.PositionID;
            _entry.ZoneKey = _info.ZoneKey;
            _entry.LocationKey = _info.LocationKey;
            _entry.ContainerID = _info.ContainerID;
            _entry.Lpn = _info.Lpn;
            _entry.Compartment = _info.Compartment;
            _entry.ItemMasterID = _info.ItemMasterID;
            _entry.Sku = _info.Sku;
            _entry.StateKey = _info.StateKey;
            _entry.TagTypeKey = _info.TagTypeKey;
            _entry.TaggerID = _info.TaggerID;
            _entry.FinalQuantity = _info.FinalQuantity;
            _entry.InitialQuanity = _info.InitialQuanity;
            _entry.QuantityChange = _info.QuantityChange;
            _Activity.InventoryEntries!.Add(_entry);
        }
    }

    public void AddDemands(params List<DemandEntryParam> demands)
    {
        if (_Activity == null)
        {
            throw new InvalidOperationException(@"no current activity");
        }

        var _now = DateTimeOffset.Now;
        var _milli = _Timer!.ElapsedMilliseconds;
        var _micro = (long)_Timer!.Elapsed.TotalMicroseconds;
        var _index = GetNextEntryIndex();
        foreach (var _info in demands)
        {
            var _entry = NewEntry<DemandEntry>(_index, _now, _milli, _micro);
            _entry.EntryLine = _info.EntryLine;
            _entry.OrderID = _info.OrderID;
            _entry.OrderKey = _info.OrderKey;
            _entry.OrderStateKey = _info.OrderStateKey;
            _entry.EntryValue = _info.EntryValue;
            _entry.DemandID = _info.DemandID;
            _entry.DemandTypeKey = _info.DemandTypeKey;
            _entry.DemandStateKey = _info.DemandStateKey;
            _entry.ItemMasterID = _info.ItemMasterID;
            _entry.Sku = _info.Sku;
            _entry.StageID = _info.StageID;
            _entry.StageTypeKey = _info.StageTypeKey;
            _entry.StageStateKey = _info.StageStateKey;
            _entry.PackageID = _info.PackageID;
            _entry.PackageLpn = _info.PackageLpn;
            _Activity.DemandEntries!.Add(_entry);
        }
    }

    public void AddFulfillments(params List<FulfillmentEntryParam> fulfillments)
    {
        if (_Activity == null)
        {
            throw new InvalidOperationException(@"no current activity");
        }

        var _now = DateTimeOffset.Now;
        var _milli = _Timer!.ElapsedMilliseconds;
        var _micro = (long)_Timer!.Elapsed.TotalMicroseconds;
        var _index = GetNextEntryIndex();
        foreach (var _info in fulfillments)
        {
            var _entry = NewEntry<FulfillmentEntry>(_index, _now, _milli, _micro);
            _entry.EntryLine = _info.EntryLine;
            _entry.SourceKey = _info.SourceKey;
            _entry.DestinationKey = _info.DestinationKey;
            _entry.UnitPoolKey = _info.UnitPoolKey;
            _entry.ItemMasterID = _info.ItemMasterID;
            _entry.Sku = _info.Sku;
            _entry.ChannelTypeKey = _info.ChannelTypeKey;
            _entry.PacketID = _info.PacketID;
            _entry.ZoneID = _info.ZoneID;
            _entry.ZoneKey = _info.ZoneKey;
            _entry.PositionID = _info.PositionID;
            _entry.LocationKey = _info.LocationKey;
            _entry.ContainerID = _info.ContainerID;
            _entry.Compartment = _info.Compartment;
            _entry.EntryValue = _info.EntryValue;
            _Activity.FulfillmentEntries!.Add(_entry);
        }
    }

    public void AddException(Exception exception)
    {
        if (_Activity == null)
        {
            throw new InvalidOperationException(@"no current activity");
        }

        var _now = DateTimeOffset.Now;
        var _milli = _Timer!.ElapsedMilliseconds;
        var _micro = (long)_Timer!.Elapsed.TotalMicroseconds;
        var _entry = NewEntry<ExceptionEntry>(GetNextEntryIndex(), _now, _milli, _micro);
        _entry.ExceptionName = new(exception.GetType().Name);
        _entry.Message = new(exception.Message);
        _entry.StackTrace = exception.StackTrace;
        _Activity.ExceptionEntries!.Add(_entry);
    }
}
