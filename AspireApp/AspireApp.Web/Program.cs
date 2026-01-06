using AspireApp.Web;
using AspireApp.Web.Components;
using Phaneritic.Implementations.Startup;
using Microsoft.Extensions.Hosting;

var _builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
_builder.AddServiceDefaults();

_builder.Configuration
    .AddJsonFile($@"appsettings.{Environment.MachineName}.json", true, true)
    .AddJsonFile($@"appsettings.{Environment.GetEnvironmentVariable(@"USECONFIG")}.json", true, true)
    .AddEnvironmentVariables();

// Add services to the container.
var _startup = new Startup(_builder.Configuration);
_startup.ConfigureServices(_builder.Services);

// Add services to the container.
_builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

_builder.Services.AddOutputCache();

var _app = (_builder
    .Build()
    .StartUp() as WebApplication)!;

if (!_app.Environment.IsDevelopment())
{
    _app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    _app.UseHsts();
}

_app.UseHttpsRedirection();

_app.UseAntiforgery();

_app.UseOutputCache();

_app.MapStaticAssets();

_app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

_app.MapDefaultEndpoints();

_app.Run();
