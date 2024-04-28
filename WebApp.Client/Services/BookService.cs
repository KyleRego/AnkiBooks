using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class BookService(HttpClient httpClient) : HttpServiceBase(httpClient), IBookService
{
    public async Task<List<Book>?> GetPublicBooks()
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/Books");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Book>>(responseBody, _jsonOptions);
    }
}
