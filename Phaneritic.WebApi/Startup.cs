using Phaneritic.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Phaneritic.WebApi;

public class Startup(
    IConfiguration configuration
    )
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.ConfigureKernel(Configuration);
        services.AddPhaneriticKernel();

        // JWT Authentication
        var jwtKey = Configuration["Jwt:Key"] ?? string.Empty;
        var jwtIssuer = Configuration["Jwt:Issuer"];
        var jwtAudience = Configuration["Jwt:Audience"];

        if (!string.IsNullOrEmpty(jwtKey))
        {
            var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = !string.IsNullOrEmpty(jwtIssuer),
                    ValidIssuer = jwtIssuer,
                    ValidateAudience = !string.IsNullOrEmpty(jwtAudience),
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true
                };
            });

            services.AddAuthorization();
            services.AddAuthentication();

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
        }
    }
}
