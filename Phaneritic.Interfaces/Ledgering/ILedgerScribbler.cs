using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Interfaces.Ledgering;
public interface ILedgerScribbler
{
    void BeginActivity(ActivityTypeKey activityTypeKey, MethodKey methodKey, bool forceRestart = false);
    void CompleteActivity(bool isSuccessful = true);
    void ClearActivity();

    ActivityDto? ActiveActivity { get; }

    bool HasYieldedToOuter { get; }

    /// <summary>
    /// Subsequent entries will be written into activity of the provided <see cref="ILedgerScribbler">ILedgerScribbler</see>
    /// </summary>
    void YieldToOuterActivity(ILedgerScribbler ledgerScribbler);

    void AddException(Exception exception);

    /// <remarks>Each entry must have a unique InfoEntryKey in a single call.</remarks>
    void AddInfo(params List<InfoEntryParam> informations);
}
