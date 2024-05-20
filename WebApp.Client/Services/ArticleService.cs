using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class ArticleService(HttpClient httpClient) : HttpServiceBase(httpClient), IArticleService
{
    public async Task<Article?> GetArticle(string articleId)
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/Articles/{articleId}");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }

    public async Task<List<Article>?> GetUserArticles()
    {
        HttpRequestMessage request = new(HttpMethod.Get, $"api/UserArticles");
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<Article>>(responseBody, _jsonOptions);
    }

    public async Task<Article?> PostArticle(Article article)
    {
        HttpRequestMessage request = new(HttpMethod.Post, $"api/Articles")
        {
            Content = new StringContent(JsonSerializer.Serialize(article),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }

    public async Task<Article?> PatchArticle(Article article)
    {
        HttpRequestMessage request = new(HttpMethod.Patch, $"api/Articles/{article.Id}")
        {
            Content = new StringContent(JsonSerializer.Serialize(article),
                                        new MediaTypeHeaderValue("application/json"))
        };
        request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);

        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }
}