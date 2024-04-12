using System.Net.Http.Json;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Client;

public interface IAnkiBooksApiService
{
    public Task<Article?> GetArticle(string articleId);
    public Task<Article[]?> GetArticles();
    public Task<Article?> PostArticle(Article articleData);

    public Task<IArticleElement?> PostArticleElement(IArticleElement element);
    public Task<IArticleElement?> PutArticleElement(IArticleElement element);
    public Task DeleteArticleElement(IArticleElement element);
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

    public async Task<IArticleElement?> PostArticleElement(IArticleElement element)
    {
        if (element is BasicNote basicNote)
        {
            return await PostBasicNote(basicNote);
        }
        else if (element is ClozeNote clozeNote)
        {
            return await PostClozeNote(clozeNote);
        }
        else if (element is MarkdownContent content)
        {
            return await PostMarkdownContent(content);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public async Task<IArticleElement?> PutArticleElement(IArticleElement element)
    {
        if (element is BasicNote basicNote)
        {
            return await PutBasicNote(basicNote);
        }
        else if (element is ClozeNote clozeNote)
        {
            return await PutClozeNote(clozeNote);
        }
        else if (element is MarkdownContent content)
        {
            return await PutMarkdownContent(content);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public async Task DeleteArticleElement(IArticleElement element)
    {
        if (element is BasicNote basicNote)
        {
            await DeleteBasicNote(basicNote.Id);
        }
        else if (element is ClozeNote clozeNote)
        {
            await DeleteClozeNote(clozeNote.Id);
        }
        else if (element is MarkdownContent content)
        {
            await DeleteMarkdownContent(content.Id);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    private async Task<BasicNote?> PostBasicNote(BasicNote bnData)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/BasicNotes", bnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    private async Task<BasicNote?> PutBasicNote(BasicNote bnData)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/BasicNotes/{bnData.Id}", bnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    private async Task DeleteBasicNote(string basicNoteId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/BasicNotes/{basicNoteId}");
        response.EnsureSuccessStatusCode();
    }

    private async Task<ClozeNote?> PostClozeNote(ClozeNote cnData)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ClozeNotes", cnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);
    }

    private async Task<ClozeNote?> PutClozeNote(ClozeNote cnData)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/ClozeNotes/{cnData.Id}", cnData);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);        
    }

    private async Task DeleteClozeNote(string clozeNoteId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/ClozeNotes/{clozeNoteId}");
        response.EnsureSuccessStatusCode();
    }

    private async Task<MarkdownContent?> PostMarkdownContent(MarkdownContent markdownContent)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/MarkdownContents", markdownContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);        
    }

    private async Task<MarkdownContent?> PutMarkdownContent(MarkdownContent markdownContent)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/MarkdownContents/{markdownContent.Id}",
                                                                        markdownContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);        
    }

    private async Task DeleteMarkdownContent(string contentId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/MarkdownContents/{contentId}");
        response.EnsureSuccessStatusCode();
    }
}