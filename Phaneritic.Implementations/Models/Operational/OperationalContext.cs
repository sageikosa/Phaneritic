using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Phaneritic.Implementations.EF;
using Phaneritic.Interfaces;
using Phaneritic.Interfaces.Database;
using Phaneritic.Interfaces.Operational;

namespace Phaneritic.Implementations.Models.Operational;
public class OperationalContext(
    ILoggerFactory loggerFactory,
    IEnumerable<IPropertyConfigurator> configurators,
    IEnumerable<IDbSchemaProvider> schemaNamers,
    IDbScopedConnection connection
    ) : BaseDbContext(loggerFactory, configurators, schemaNamers, connection),
    IOperationalContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasSequence(nameof(AccessorID)).StartsAt(10000).IncrementsBy(10);
        modelBuilder.HasSequence(nameof(AccessMechanismID)).StartsAt(10000).IncrementsBy(10);
        modelBuilder.HasSequence(nameof(OperationID)).StartsAt(100000).IncrementsBy(100);
        modelBuilder.HasSequence(nameof(OperationLogID)).StartsAt(100000).IncrementsBy(100);
        modelBuilder.HasSequence(nameof(AccessSessionID)).StartsAt(100000).IncrementsBy(100);

        modelBuilder.Entity<Accessor>().Property(_e => _e.AccessorID).UseHiLo(nameof(AccessorID));
        modelBuilder.Entity<AccessMechanism>().Property(_e => _e.AccessMechanismID).UseHiLo(nameof(AccessMechanismID));
        modelBuilder.Entity<Operation>().Property(_e => _e.OperationID).UseHiLo(nameof(OperationID));
        modelBuilder.Entity<OperationLog>().Property(_e => _e.OperationLogID).UseHiLo(nameof(OperationLogID));
        modelBuilder.Entity<AccessSession>().Property(_e => _e.AccessSessionID).UseHiLo(nameof(AccessSessionID));
    }

    public DbSet<ProcessNodeType> ProcessNodeTypes { get; set; }
    public DbSet<ProcessNode> ProcessNodes { get; set; }
    public DbSet<OptionGroup> OptionGroups { get; set; }
    public DbSet<ProcessNodeTypeOptionGroup> ProcessNodeTypeOptionGroups { get; set; }
    public DbSet<OptionType> OptionTypes { get; set; }
    public DbSet<OptionGroupOptionType> OptionGroupOptionTypes { get; set; }
    public DbSet<Option> Options { get; set; }

    public DbSet<AccessMechanism> AccessMechanisms { get; set; }
    public DbSet<AccessMechanismType> AccessMechanismTypes { get; set; }
    public DbSet<Accessor> Accessors { get; set; }
    public DbSet<AccessorType> AccessorTypes { get; set; }
    public DbSet<AccessorTypeAccessMechanismType> AccessorTypeAccessMechanismTypes { get; set; }

    // Added: credential types and credentials
    public DbSet<AccessorCredentialType> AccessorCredentialTypes { get; set; }
    public DbSet<AccessorCredential> AccessorCredentials { get; set; }
    public DbSet<AccessorTypeAccessorCredentialType> AccessorTypeAccessorCredentialTypes { get; set; }

    public DbSet<AccessSession> AccessSessions { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<OperationLog> OperationLogs { get; set; }
    public DbSet<Method> Methods { get; set; }
    public DbSet<MethodAccessMechanismType> MethodAccessMechanismTypes { get; set; }

    public DbSet<AccessGroup> AccessGroups { get; set; }
    public DbSet<AccessGroupMethod> AccessGroupMethods { get; set; }
    public DbSet<AccessorAccessGroup> AccessorAccessGroups { get; set; }

    protected override string DefaultSchema => @"op";
}
