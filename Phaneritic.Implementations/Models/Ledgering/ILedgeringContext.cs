using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Implementations.Models.Ledgering;
public interface ILedgeringContext : IContributeWork
{
    DbSet<Activity> Activities { get; }
    DbSet<ActivityType> ActivityTypes { get; }
    DbSet<InfoEntry> InfoEntries { get; }
    DbSet<ExceptionEntry> ExceptionEntries { get; }
}
