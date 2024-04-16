using AnkiBooks.WebApp.Client.Services;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace AnkiBooks.WebApp.Client;

/// <summary>
/// Due to prerendering on the server, some services needed in Program.cs
/// in WebApp.Client also need to be registered in WebApp
/// </summary>
public static class CommonServices
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(configuration["AppUrl"]!) });
        services.AddScoped<IAnkiBooksApiService, AnkiBooksApiService>();
        services.AddScoped<DraggedItemHolder<INote>>();
    }
}