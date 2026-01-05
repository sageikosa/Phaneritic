namespace Phaneritic.Interfaces.Operational;
public interface IAccessMechanismReader
{
    AccessMechanismDto? GetScopedAccessMechanism();
    AccessMechanismDto? GetAccessMechanism(AccessMechanismID accessMechanismID);
    AccessMechanismDto? GetAccessMechanism(AccessMechanismKey accessMechanismKey);
}
