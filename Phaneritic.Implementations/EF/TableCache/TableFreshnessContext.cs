using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Database;

namespace Phaneritic.Implementations.EF.TableCache;

public class TableFreshnessContext(
    ILoggerFactory loggerFactory,
    IEnumerable<IPropertyConfigurator> configurators,
    IEnumerable<IDbSchemaProvider> schemaNamers,
    IDbScopedConnection connection)
    : BaseDbContext(loggerFactory, configurators, schemaNamers, connection), ITableFreshnessContextTransient
{
    protected override string DefaultSchema => @"krnl";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var _schema = GetSchemaName();
        modelBuilder.HasDefaultSchema(_schema);

        modelBuilder.Entity<TableFreshness>().ToTable(nameof(TableFreshness));
        modelBuilder.Entity<TableFreshness>().HasKey(_lf => _lf.TableKey);
        modelBuilder.Entity<TableFreshness>().Property(_lf => _lf.ConcurrencyCheck).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<TableFreshness> TableFreshnesses { get; set; } = null!;
}
