using AnkiBooks.WebApp.Client;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["Url"]!) });
builder.Services.AddRadzenComponents();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();
