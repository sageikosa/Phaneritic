using System.Collections.Frozen;

namespace Phaneritic.Interfaces.Operational;
public record AccessorDto
{
    public AccessorID AccessorID { get; init; }
    public AccessorKey AccessorKey { get; init; }
    public DescriptionString Description { get; init; }
    public AccessorTypeKey AccessorTypeKey { get; init; }

    public FrozenSet<AccessGroupDto> AccessGroups { get; init; } = [];
    public FrozenSet<AccessorCredentialDto> Credentials { get; init; } = [];
}
