using System.Net.Http.Json;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Client;

public interface IAnkiBooksApiService
{
    public Task<Article?> GetArticle(string articleId);
    public Task<Article[]?> GetArticles();
    public Task<Article?> PostArticle(Article articleData);
    public Task<BasicNote?> PostBasicNote(BasicNote bnData);
    public Task<BasicNote?> PutBasicNote(BasicNote bnData);
    public Task<ClozeNote?> PostClozeNote(ClozeNote cnData);
}

public class AnkiBooksApiService(HttpClient httpClient) : IAnkiBooksApiService
{
    private readonly HttpClient _httpClient = httpClient;

    private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

    public async Task<Article?> GetArticle(string articleId)
    {
        return await _httpClient.GetFromJsonAsync<Article>($"api/Articles/{articleId}");
    }

    public async Task<Article[]?> GetArticles()
    {
        return await _httpClient.GetFromJsonAsync<Article[]>("api/Articles");
    }

    public async Task<Article?> PostArticle(Article articleData)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Articles", articleData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Article>(responseBody, _jsonOptions);
    }

    public async Task<BasicNote?> PostBasicNote(BasicNote bnData)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/BasicNotes", bnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    public async Task<BasicNote?> PutBasicNote(BasicNote bnData)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/BasicNotes/{bnData.Id}", bnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    public async Task<ClozeNote?> PostClozeNote(ClozeNote cnData)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ClozeNotes", cnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);
    }
}