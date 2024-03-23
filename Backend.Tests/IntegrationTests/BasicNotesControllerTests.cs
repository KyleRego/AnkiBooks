using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Backend.DbContext;
using AnkiBooks.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace AnkiBooks.Backend.Tests.IntegrationTests;

public class BasicNotesControllerTests : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory;

    public BasicNotesControllerTests(TestServerFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Post_BasicNotes_CreatesBasicNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 0,
            ArticleId = article.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Post_BasicNotes_CreatesBasicNoteAtOrdinalPositionAndShiftsOthers()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        List<BasicNote> existingBasicNotes = new()
        {
            new() { Front = "Front", Back = "Back", OrdinalPosition = 0 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 1 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 2 }
        };
        
        article.BasicNotes = existingBasicNotes;
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 1,
            ArticleId = article.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}