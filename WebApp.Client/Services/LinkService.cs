using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class LinkService(HttpClient httpClient) : HttpServiceBase(httpClient), ILinkService
{
    public async Task<List<Link>?> GetLinks()
    {
        HttpRequestMessage request = new(HttpMethod.Get, "api/Links");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Link>>(responseBody, _jsonOptions);
    }

    public async Task<Link?> PostLink(Link link)
    {
        HttpRequestMessage request = new(HttpMethod.Post, "api/Links")
        {
            Content = new StringContent(JsonSerializer.Serialize(link),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Link>(responseBody, _jsonOptions);
    }

    public async Task<Link?> PutLink(Link link)
    {
        HttpRequestMessage request = new(HttpMethod.Put, $"api/Links/{link.Id}")
        {
            Content = new StringContent(JsonSerializer.Serialize(link),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Link>(responseBody, _jsonOptions);
    }
}