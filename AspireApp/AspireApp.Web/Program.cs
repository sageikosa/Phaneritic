using AspireApp.Web;
using AspireApp.Web.Components;
using Microsoft.Extensions.Hosting;

var _builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire client integrations.
_builder.AddServiceDefaults();

// Add services to the container.
_builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

_builder.Services.AddOutputCache();

var _app = _builder.Build();

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
