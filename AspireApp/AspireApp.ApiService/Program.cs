using AspireApp.Web;
using Microsoft.Extensions.Hosting;
using Phaneritic.Implementations.Startup;

var _builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
_builder.AddServiceDefaults();

_builder.Configuration
    .AddJsonFile($@"appsettings.{Environment.MachineName}.json", true, true)
    .AddJsonFile($@"appsettings.{Environment.GetEnvironmentVariable(@"USECONFIG")}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.
_builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
_builder.Services.AddOpenApi();

// Add services to the container.
var _startup = new Startup(_builder.Configuration);
_startup.ConfigureServices(_builder.Services);

var _app = (_builder
    .Build()
    .StartUp() as WebApplication)!;

// Configure the HTTP request pipeline.
_app.UseExceptionHandler();

if (_app.Environment.IsDevelopment())
{
    _app.MapOpenApi();
}

_app.MapDefaultEndpoints();

_app.Run();