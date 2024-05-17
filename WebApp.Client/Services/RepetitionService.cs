using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class RepetitionService(HttpClient client) : HttpServiceBase(client), IRepetitionService
{
    public async Task<Repetition?> PostRepetition(Repetition rep)
    {
        HttpRequestMessage request = new(HttpMethod.Post, "api/Repetitions")
        {
            Content = new StringContent(JsonSerializer.Serialize(rep),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Repetition>(responseBody, _jsonOptions);
    }
}