using System.Text.Json;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Components.WebAssembly.Http;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Services;

public class CardService(HttpClient httpClient) : HttpServiceBase(httpClient), ICardService
{
    public async Task<Card?> PostCard(Card card)
    {
        switch(card)
        {
            case BasicNote bn:
                return await PostBasicNote(bn);

            case ClozeNote cn:
                return await PostClozeNote(cn);

            default:
                throw new ApplicationException();
        }
    }

    private async Task<BasicNote?> PostBasicNote(BasicNote basicNote)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/BasicNotes", basicNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    private async Task<ClozeNote?> PostClozeNote(ClozeNote clozeNote)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ClozeNotes", clozeNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);
    }

    public async Task<Card?> PutCard(Card card)
    {
        switch(card)
        {
            case BasicNote bn:
                return await PutBasicNote(bn);

            case ClozeNote cn:
                return await PutClozeNote(cn);

            default:
                throw new ApplicationException();
        }
    }

    private async Task<BasicNote?> PutBasicNote(BasicNote basicNote)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/BasicNotes/{basicNote.Id}", basicNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<BasicNote>(responseBody, _jsonOptions);
    }

    private async Task<ClozeNote?> PutClozeNote(ClozeNote clozeNote)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/ClozeNotes/{clozeNote.Id}", clozeNote);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ClozeNote>(responseBody, _jsonOptions);
    }

    public async Task DeleteCard(Card card)
    {
        switch(card)
        {
            case BasicNote bn:
                await DeleteBasicNote(bn.Id);
                return;

            case ClozeNote cn:
                await DeleteClozeNote(cn.Id);
                return;

            default:
                throw new ApplicationException();
        }
    }

    private async Task DeleteBasicNote(string basicNoteId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/BasicNotes/{basicNoteId}");
        response.EnsureSuccessStatusCode();
    }

    private async Task DeleteClozeNote(string clozeNoteId)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync($"api/ClozeNotes/{clozeNoteId}");
        response.EnsureSuccessStatusCode();
    }
}