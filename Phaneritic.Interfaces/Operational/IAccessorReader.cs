namespace Phaneritic.Interfaces.Operational;
public interface IAccessorReader
{
    AccessorDto? GetScopedAccessor();
    AccessorDto? GetAccessor(AccessorID accessorID);
    AccessorDto? GetAccessor(AccessorKey accessorKey);
    List<AccessorDto> GetAccessors(AccessorTypeKey accessorTypeKey);
}
