using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Mvc.Testing;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Tests.Controllers.BasicNotesControllerTests;

public class BasicNoteControllerTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task PostBasicNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Deck deck = await dbContext.CreateDeck();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            DeckId = deck.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PutBasicNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        BasicNote basicNote = new() { Front = "Hello", Back = "World" };
        Deck deck = await dbContext.CreateDeck();
        deck.BasicNotes.Add(basicNote);
        await dbContext.SaveChangesAsync();

        BasicNote bnData = new()
        {
            Id = basicNote.Id,
            Front = "Front",
            Back = "Back",
            DeckId = deck.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PutAsJsonAsync($"api/BasicNotes/{basicNote.Id}", bnData);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        dbContext.ChangeTracker.Clear();
        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("Front", updatedBasicNote.Front);
        Assert.Equal("Back", updatedBasicNote.Back); 
    }
}