using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using AnkiBooks.WebApp.Client;
using AnkiBooks.WebApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

CommonServices.Configure(builder.Services, builder.Configuration);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped<IUserArticleService, ClientUserArticleService>();

await builder.Build().RunAsync();
