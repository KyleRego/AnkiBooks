using System.Net.Http.Json;
using System.Text.Json;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Services;

public class ArticleElementService(HttpClient httpClient) : HttpServiceBase(httpClient), IArticleElementService
{
    public async Task DeleteArticleElement(ArticleElement artElement)
    {
        switch(artElement)
        {
            case MarkdownContent md:
                await DeleteMarkdownContent(md.Id);
                return;

            case Deck deck:
                await DeleteDeck(deck.Id);
                return;

            default:
                throw new ApplicationException();
        }
    }

    public async Task<ArticleElement?> PostArticleElement(ArticleElement artElement)
    {
        switch(artElement)
        {
            case MarkdownContent md:
                return await PostMarkdownContent(md);

            case Deck deck:
                return await PostDeck(deck);

            default:
                throw new ApplicationException();
        }
    }

    public async Task<ArticleElement?> PutArticleElement(ArticleElement artElement)
    {
        switch(artElement)
        {
            case MarkdownContent md:
                return await PutMarkdownContent(md);

            case Deck deck:
                return await PutDeck(deck);

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

    private async Task<Deck?> PostDeck(Deck deck)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Decks", deck);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Deck>(responseBody, _jsonOptions);
    }

    private async Task<Deck?> PutDeck(Deck deck)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Decks/{deck.Id}", deck);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Deck>(responseBody, _jsonOptions);
    }

    private async Task DeleteDeck(string deckId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Decks/{deckId}");
        response.EnsureSuccessStatusCode();
    }
}