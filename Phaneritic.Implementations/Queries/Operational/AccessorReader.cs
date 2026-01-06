using Phaneritic.Implementations.Operational;
using Phaneritic.Interfaces.Operational;
using Microsoft.EntityFrameworkCore;
using Phaneritic.Implementations.Models.Operational;
using Phaneritic.Interfaces;

namespace Phaneritic.Implementations.Queries.Operational;
public class AccessorReader(
    IOperationalContext operationalContext,
    IPackRecord<Accessor, AccessorDto> accessorPacker,
    IEnumerable<IProvideAccessor> provideAccessors
    ) : IAccessorReader
{
    protected IQueryable<Accessor> BaseQuery()
        => operationalContext.Accessors
        .Include(_a => _a.AccessGroups)
        .Include(_a => _a.Credentials);

    public AccessorDto? GetAccessor(AccessorID accessorID)
        => accessorPacker.Pack(BaseQuery().FirstOrDefault(_a => _a.AccessorID == accessorID));

    public AccessorDto? GetAccessor(AccessorKey accessorKey)
        => accessorPacker.Pack(BaseQuery().FirstOrDefault(_a => _a.AccessorKey == accessorKey));

    public AccessorDto? GetScopedAccessor()
        => provideAccessors.GetScopedAccessor();

    public List<AccessorDto> GetAccessors(AccessorTypeKey accessorTypeKey)
        => [.. accessorPacker.GetDtos(BaseQuery().Where(_a => _a.AccessorTypeKey == accessorTypeKey))];
}
