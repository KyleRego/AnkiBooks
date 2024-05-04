using System.Text.Json;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Components.WebAssembly.Http;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Services;

public class CardService(HttpClient httpClient) : HttpServiceBase(httpClient), ICardService
{
    public async Task<Card> PostCard(Card card)
    {
        switch(card)
        {
            case BasicNote bn:
                return (Card)(await PostBasicNote(bn));
                break;

            case ClozeNote cn:
                return (Card)(await PostClozeNote(cn));
                break;

            default:
                throw new ApplicationException();
        }
    }

    private async Task<BasicNote> PostBasicNote(BasicNote basicNote)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/BasicNotes", basicNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    private async Task<ClozeNote> PostClozeNote(ClozeNote clozeNote)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ClozeNotes", clozeNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);
    }

    public Task<Card> PutCard(Card card)
    {
        throw new NotImplementedException();
    }

    public Task DeleteCard(string cardId)
    {
        throw new NotImplementedException();
    }
}