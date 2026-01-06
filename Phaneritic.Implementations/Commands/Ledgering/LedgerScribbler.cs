using Phaneritic.Implementations.Commands.Operational;
using Phaneritic.Implementations.Models.Ledgering;
using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.CommitWork;
using Phaneritic.Interfaces.Ledgering;
using Phaneritic.Interfaces.LudCache;
using Phaneritic.Interfaces.Operational;
using System.Diagnostics;

namespace Phaneritic.Implementations.Commands.Ledgering;

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
