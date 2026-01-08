using Microsoft.Extensions.Hosting;
using Phaneritic.Implementations.Startup;
using Phaneritic.WebApi;
using Scalar.AspNetCore;
using Serilog;

var _builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var _startup = new Startup(_builder.Configuration);
_startup.ConfigureServices(_builder.Services);

_builder.Services.AddControllers();
_builder.Host.UseSerilog((ctx, svcs, config) => config.ReadFrom.Configuration(ctx.Configuration));

_builder.Configuration
    .AddJsonFile($@"appsettings.{Environment.MachineName}.json", true, true)
    .AddJsonFile($@"appsettings.{Environment.GetEnvironmentVariable(@"USECONFIG")}.json", true, true)
    .AddEnvironmentVariables();

//// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//_builder.Services.AddOpenApi();

var _app = (_builder
    .Build()
    .StartUp() as WebApplication)!;

// Configure the HTTP request pipeline.
if (_app.Environment.IsDevelopment())
{
    _app.MapOpenApi();
    _app.MapScalarApiReference(options =>
    {
        options.Authentication =
            new ScalarAuthenticationOptions
            {
                PreferredSecuritySchemes = ["Bearer"]
            };
    });
}

_app.UseHttpsRedirection();
_app.UseMiddleware<LedgerCloserMiddleware>();
_app.UseMiddleware<ErrorCatchMiddleware>();
_app.UseAuthentication();
_app.UseAuthorization();

_app.MapControllers();

_app.Run();
