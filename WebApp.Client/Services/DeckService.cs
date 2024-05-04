using System.Net.Http.Json;
using System.Text.Json;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Services;

public class DeckService(HttpClient httpClient) : HttpServiceBase(httpClient), IDeckService
{
    public async Task<Deck?> PostDeck(Deck deck)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Decks", deck);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Deck>(responseBody, _jsonOptions);
    }

    public async Task<Deck?> PutDeck(Deck deck)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Decks/{deck.Id}", deck);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Deck>(responseBody, _jsonOptions);
    }

    public async Task DeleteDeck(string deckId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/Decks/{deckId}");
        response.EnsureSuccessStatusCode();
    }
}