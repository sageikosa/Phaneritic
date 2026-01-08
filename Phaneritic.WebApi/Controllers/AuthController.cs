using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Phaneritic.Interfaces.Operational;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Phaneritic.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IConfiguration config
    ) : ControllerBase
{
    [HttpPost("token")]
    [AllowAnonymous]
    public IActionResult GetTokenFromAccessorSecret(
        [FromServices] IAccessorReader accessorReader,
        [FromBody] LoginRequest request)
    {
        // very basic username/password check - replace with real validation
        var _accessorKey = new AccessorKey(request?.Username ?? string.Empty);
        if (accessorReader.GetAccessor(_accessorKey) is AccessorDto _accessor)
        {
            if (_accessor.Credentials.FirstOrDefault(_c => _c.AccessorCredentialTypeKey == AccessorCredentialTypeKey.Secret) is AccessorCredentialDto _credential)
            {
                // found credential, now check it
                // TODO: replace with real password hashing and verification
                if (_credential.CredentialValue.KeyVal != request?.Password)
                {
                    return Unauthorized();
                }
            }
            else
            {
                return Unauthorized();
            }

            var _key = config["Jwt:Key"] ?? string.Empty;
            var _issuer = config["Jwt:Issuer"];
            var _audience = config["Jwt:Audience"];
            var _claims = new[]
            {
                new Claim(ClaimTypes.Name, _accessor.AccessorKey),
            };
            var _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key));
            var _creds = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha256);
            var _token = new JwtSecurityToken(_issuer, _audience, _claims, expires: DateTime.UtcNow.AddHours(1), signingCredentials: _creds);
            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(_token) });
        }
        return Unauthorized();
    }

    public record LoginRequest(string Username, string Password);
}
