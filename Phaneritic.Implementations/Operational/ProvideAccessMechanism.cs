using Phaneritic.Interfaces.Operational;
using Microsoft.Extensions.Logging;
using Phaneritic.Implementations.Operational;
using System.Security.Claims;
using Phaneritic.Interfaces.LudCache;
using Microsoft.AspNetCore.Http;

namespace Phaneritic.Implementations.Operational;
public class ProvideAccessMechanism(
    ILudDictionary<AccessMechanismTypeKey, AccessMechanismTypeDto> accessMechanismTypes,
    ILudDictionary<AccessMechanismKey, AccessMechanismDto> accessMechanisms,
    IHttpContextAccessor httpContext,
    ILogger<IProvideAccessMechanism> logger
    ) : IProvideAccessMechanism
{
    private AccessMechanismDto? _Current;

    public int Priority => 100;

    public AccessMechanismDto? CurrentAccessMechanism
    {
        get
        {
            // get if not set
            if (_Current == null)
            {
                // try to get from claims principal (asserting System claim as AccessMechanismKey)
                if (httpContext.HttpContext?.User is ClaimsPrincipal _claims)
                {
                    // this will only work if something else has established a System claim
                    if (_claims.Claims.FirstOrDefault(_c => _c.Type == ClaimTypes.System) is Claim _sys)
                    {
                        _Current = accessMechanisms.Get(new AccessMechanismKey(_sys.Value));
                    }
                }
                else if (accessMechanismTypes.All().Any(_amt => _amt.IsValidatedIPAddress))
                {
                    var _mechanismKey = new AccessMechanismKey(httpContext.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? string.Empty);
                    if ((accessMechanisms.Get(_mechanismKey) is AccessMechanismDto _mechanism)
                        && _mechanism.AccessMechanismType.IsValidatedIPAddress)
                    {
                        // found validated mechanism named by the ip-address
                        _Current = _mechanism;
                    }
                }
                else
                {
                    logger.LogWarning(@"no claim nor IP address to use as an access mechanism");
                }
            }

            return _Current;
        }
    }
}
