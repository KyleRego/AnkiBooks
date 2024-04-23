using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class ClientUserArticleService(HttpClient httpClient) : IUserArticleService
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

    public async Task<Article?> GetUserArticle(string articleId)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/user/Articles/{articleId}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }

    public async Task<List<Article>?> GetUserArticles()
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/user/Articles");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Article>>(responseBody, _jsonOptions);
    }

    public async Task<Article?> PostUserArticle(Article articleData)
    {
        HttpRequestMessage request = new(HttpMethod.Post, $"api/user/Articles")
        {
            Content = new StringContent(JsonSerializer.Serialize(articleData),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }
}