using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Mvc.Testing;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Tests.Controllers.ClozeNotesControllerTests;

public class ClozeNotesControllerTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task PostClozeNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Deck deck = await dbContext.CreateDeck();

        ClozeNote clozeNote = new()
        {
            Text = "Content",
            DeckId = deck.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/ClozeNotes", clozeNote);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}