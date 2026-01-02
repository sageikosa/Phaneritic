using GyroLedger.CodeInterface;
using GyroLedger.CodeInterface.CommitWork;
using GyroLedger.CodeInterface.Database;
using GyroLedger.Kernel.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GyroLedger.Kernel.EF;
public abstract class BaseDbContext(
    ILoggerFactory loggerFactory,
    IEnumerable<IPropertyConfigurator> configurators,
    IEnumerable<IDbSchemaProvider> schemaNamers,
    IBaseDbConnection connection) 
    : DbContext, IContributeWork
{
    protected abstract string DefaultSchema { get; }
    protected virtual string MigrationsName => @"__MigrationsHistory";

    protected virtual string GetSchemaName()
        => schemaNamers.GetSchemaName(GetType().FullName!, DefaultSchema);

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            // TODO: conditionally add this (can also use logging level options...)
            .UseLoggerFactory(loggerFactory)
            .UseSqlServer(connection.Connection,
            opt =>
            {
                opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                opt.MigrationsHistoryTable(
                    MigrationsName,
                    GetSchemaName());
            });
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(GetSchemaName());
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        foreach (var _conf in configurators)
        {
            _conf.ConfigureProperties(configurationBuilder);
        }
        configurationBuilder.Properties<decimal>().HavePrecision(18, 4);
        configurationBuilder.Properties<decimal?>().HavePrecision(18, 4);
        base.ConfigureConventions(configurationBuilder);
    }

    public virtual IEnumerable<IContributeWork> ContributeWork()
    {
        SaveChanges(false);
        yield break;
    }

    public virtual IEnumerable<IContributeWork> ContributeAfterWork()
    {
        ChangeTracker.AcceptAllChanges();
        yield break;
    }
}
