using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace AnkiBooks.WebApp.Client;

public interface IAnkiBooksApiService
{
    public Task<ArticleElement?> PostElement(ArticleElement element);
    public Task<ArticleElement?> PutElement(ArticleElement element);
    public Task DeleteElement(ArticleElement element);
}

public class AnkiBooksApiService(HttpClient httpClient, ILogger<AnkiBooksApiService> logger) : IAnkiBooksApiService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly ILogger<AnkiBooksApiService> _logger = logger;

    private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

    public async Task<ArticleElement?> PostElement(ArticleElement element)
    {
        switch(element)
        {
            case BasicNote bn:
                return await PostBasicNote(bn);

            case ClozeNote cn:
                return await PostClozeNote(cn);

            case MarkdownContent mdc:
                return await PostMarkdownContent(mdc);

            default:
                throw new ApplicationException();
        }
    }

    public async Task<ArticleElement?> PutElement(ArticleElement element)
    {
        switch(element)
        {
            case BasicNote bn:
                return await PutBasicNote(bn);

            case ClozeNote cn:
                return await PutClozeNote(cn);

            case MarkdownContent mdc:
                return await PutMarkdownContent(mdc);

            default:
                throw new ApplicationException();
        }
    }

    public async Task DeleteElement(ArticleElement element)
    {
        switch(element)
        {
            case BasicNote bn:
                await DeleteBasicNote(bn.Id);
                break;

            case ClozeNote cn:
                await DeleteClozeNote(cn.Id);
                break;

            case MarkdownContent mdc:
                await DeleteMarkdownContent(mdc.Id);
                break;

            default:
                throw new ApplicationException();
        }
    }

    private async Task<MarkdownContent?> PostMarkdownContent(MarkdownContent mdContent)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/MarkdownContents", mdContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);
    }

    private async Task<MarkdownContent?> PutMarkdownContent(MarkdownContent mdContent)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/MarkdownContents/{mdContent.Id}", mdContent);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<MarkdownContent>(responseBody, _jsonOptions);
    }

    private async Task DeleteMarkdownContent(string mdContentId)
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
}