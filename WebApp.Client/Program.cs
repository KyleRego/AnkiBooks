using AnkiBooks.ApplicationCore.Services;
using AnkiBooks.WebApp.Client;
using AnkiBooks.WebApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

CommonServices.Configure(builder.Services, builder.Configuration);

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();
builder.Services.AddScoped<IUserArticleService, UserArticleService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IArticleElementService, ArticleElementService>();

await builder.Build().RunAsync();
