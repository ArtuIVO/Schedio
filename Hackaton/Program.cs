using System;
using System.Net.Http;
using Hackaton;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuración de HttpClient
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<GoogleCalendarService>();
// Configuración de autenticación con Google usando OpenID Connect
builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.Authority = "https://accounts.google.com";
    options.ProviderOptions.ClientId = "215407617898-h4i7qk42shagjq1io72cked6jrgb58l2.apps.googleusercontent.com"; // Reemplaza con tu Client ID de Google
    options.ProviderOptions.ResponseType = "id_token";
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");
});

await builder.Build().RunAsync();
