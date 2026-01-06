using Microsoft.EntityFrameworkCore;
using Phaneritic.Interfaces.CommitWork;

namespace Phaneritic.Implementations.Models.Operational;

public interface IOperationalContext : IContributeWork
{
    DbSet<ProcessNodeType> ProcessNodeTypes { get; }
    DbSet<ProcessNode> ProcessNodes { get; }
    DbSet<OptionGroup> OptionGroups { get; }
    DbSet<ProcessNodeTypeOptionGroup> ProcessNodeTypeOptionGroups { get; }
    DbSet<OptionType> OptionTypes { get; }
    DbSet<OptionGroupOptionType> OptionGroupOptionTypes { get; }
    DbSet<Option> Options { get; }

    DbSet<AccessMechanism> AccessMechanisms { get; }
    DbSet<AccessMechanismType> AccessMechanismTypes { get; }
    DbSet<Accessor> Accessors { get; }
    DbSet<AccessorType> AccessorTypes { get; }
    DbSet<AccessorTypeAccessMechanismType> AccessorTypeAccessMechanismTypes { get; }
    DbSet<AccessorTypeAccessorCredentialType> AccessorTypeAccessorCredentialTypes { get; }
    DbSet<AccessorCredentialType> AccessorCredentialTypes { get; }
    DbSet<AccessorCredential> AccessorCredentials { get; }

    DbSet<AccessSession> AccessSessions { get; }
    DbSet<Operation> Operations { get; }
    DbSet<OperationLog> OperationLogs { get; }
    DbSet<Method> Methods { get; }
    DbSet<MethodAccessMechanismType> MethodAccessMechanismTypes { get; }
    DbSet<AccessGroup> AccessGroups { get; }
    DbSet<AccessGroupMethod> AccessGroupMethods { get; }
    DbSet<AccessorAccessGroup> AccessorAccessGroups { get; }
}
