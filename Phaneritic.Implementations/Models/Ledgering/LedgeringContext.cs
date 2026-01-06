using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phaneritic.Implementations.EF;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Database;
using Phaneritic.Interfaces.Ledgering;

namespace Phaneritic.Implementations.Models.Ledgering;
public class LedgeringContext(
    ILoggerFactory loggerFactory,
    IEnumerable<IPropertyConfigurator> configurators, 
    IEnumerable<IDbSchemaProvider> schemaNamers, 
    IDbLoggingConnection connection
    ) : BaseDbContext(loggerFactory, configurators, schemaNamers, connection), ILedgeringContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasSequence(nameof(ActivityID)).StartsAt(10000).IncrementsBy(100);
        modelBuilder.Entity<Activity>().Property(_a => _a.ActivityID).UseHiLo(nameof(ActivityID));

        base.OnModelCreating(modelBuilder);
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<ActivityType> ActivityTypes { get; set; }
    public DbSet<InfoEntry> InfoEntries { get; set; }
    public DbSet<ExceptionEntry> ExceptionEntries { get; set; }

    protected override string DefaultSchema => @"ldgr";
}
