using System.Net.Http.Headers;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client.Services;

public class UserArticleService(HttpClient httpClient) : HttpServiceBase(httpClient), IUserArticleService
{
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

    public async Task<Article?> PostUserArticle(Article article)
    {
        HttpRequestMessage request = new(HttpMethod.Post, $"api/user/Articles")
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

    public async Task<Article?> PatchUserArticle(Article article)
    {
        HttpRequestMessage request = new(HttpMethod.Patch, $"api/user/Articles/{article.Id}")
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