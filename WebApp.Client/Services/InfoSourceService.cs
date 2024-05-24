using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Services;

public class InfoSourceService(HttpClient client) : HttpServiceBase(client), IInfoSourceService
{
    public async Task<List<InfoSource>?> GetInfoSources(int pageNumber)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/InfoSources?pageNumber={pageNumber}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<InfoSource>>(responseBody, _jsonOptions);
    }
}