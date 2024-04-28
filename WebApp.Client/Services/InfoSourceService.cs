using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class InfoSourceService(HttpClient httpClient) : HttpServiceBase(httpClient), IInfoSourceService
{
    public async Task<List<InfoSource>?> GetInfoSources()
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/InfoSources");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<InfoSource>>(responseBody, _jsonOptions);
    }

    public async Task<LinkSource?> PostLinkSource(LinkSource linkSource)
    {
        HttpRequestMessage request = new(HttpMethod.Post, $"api/LinkSources")
        {
            Content = new StringContent(JsonSerializer.Serialize(linkSource),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<LinkSource>(responseBody, _jsonOptions);
    }
}