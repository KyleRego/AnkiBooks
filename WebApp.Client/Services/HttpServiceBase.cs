using System.Text.Json;

namespace AnkiBooks.WebApp.Client.Services;

public class HttpServiceBase
{
    protected readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}