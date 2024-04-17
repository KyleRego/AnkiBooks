using System.Net.Http.Json;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using Markdig;

namespace AnkiBooks.WebApp.Client;

public interface IAnkiBooksApiService
{
    public Task<Article?> GetArticle(string articleId);
    public Task<Article[]?> GetArticles();
    public Task<Article?> PostArticle(Article articleData);

    public Task<INote?> PostNote(INote element);
    public Task<INote?> PutNote(INote element);
    public Task DeleteNote(INote element);

    public Task<IContent?> PutContent(IContent content);

    public Task<MarkdownContent?> PostMarkdownContent(MarkdownContent mdContent);
    public Task<MarkdownContent?> PutMarkdownContent(MarkdownContent mdContent);
    public Task DeleteMarkdownContent(string mdContentId);

    public Task<Section?> PostSection(Section section);
    public Task<Section?> PutSection(Section section);
    public Task DeleteSection(string sectionId);
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

    public async Task<INote?> PostNote(INote element)
    {
        if (element is BasicNote basicNote)
        {
            return await PostBasicNote(basicNote);
        }
        else if (element is ClozeNote clozeNote)
        {
            return await PostClozeNote(clozeNote);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public async Task<INote?> PutNote(INote element)
    {
        if (element is BasicNote basicNote)
        {
            return await PutBasicNote(basicNote);
        }
        else if (element is ClozeNote clozeNote)
        {
            return await PutClozeNote(clozeNote);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public async Task DeleteNote(INote element)
    {
        if (element is BasicNote basicNote)
        {
            await DeleteBasicNote(basicNote.Id);
        }
        else if (element is ClozeNote clozeNote)
        {
            await DeleteClozeNote(clozeNote.Id);
        }
        else
        {
            throw new ApplicationException();
        }
    }

    public async Task<IContent?> PutContent(IContent content)
    {
        switch(content)
        {
            case MarkdownContent mdContent:
                MarkdownContent? updatedMdContent = await PutMarkdownContent(mdContent);
                return updatedMdContent;

            default:
                throw new ApplicationException();
        }
    }

    public async Task<MarkdownContent?> PostMarkdownContent(MarkdownContent mdContent)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/MarkdownContents", mdContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);
    }

    public async Task<MarkdownContent?> PutMarkdownContent(MarkdownContent mdContent)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/MarkdownContents/{mdContent.Id}", mdContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);
    }

    public async Task DeleteMarkdownContent(string mdContentId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/MarkdownContents/{mdContentId}");
        response.EnsureSuccessStatusCode();
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

    public async Task<Section?> PostSection(Section section)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Sections", section);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Section>(responseBody, _jsonOptions);        
    }

    public async Task<Section?> PutSection(Section section)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Sections/{section.Id}",
                                                                        section);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Section>(responseBody, _jsonOptions);        
    }

    public async Task DeleteSection(string contentId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Sections/{contentId}");
        response.EnsureSuccessStatusCode();
    }
}