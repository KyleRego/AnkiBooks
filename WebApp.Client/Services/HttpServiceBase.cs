using System.Text.Json;

namespace AnkiBooks.WebApp.Client.Services;

public class HttpServiceBase(HttpClient httpClient)
{
    protected readonly HttpClient _httpClient = httpClient;

    protected readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}