using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Mvc.Testing;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Tests.Controllers.BasicNotesControllerTests;

public class PostBasicNoteTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task CreatesBasicNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new();
        article.Sections.Add(section);
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 0,
            SectionId = section.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ShiftsOtherBasicNotes()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new();
        article.Sections.Add(section);
        List<BasicNote> existingBasicNotes =
        [
            new() { Front = "Front", Back = "Back", OrdinalPosition = 0 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 1 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 2 }
        ];
        
        section.BasicNotes = existingBasicNotes;
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 1,
            SectionId = section.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ShiftsBasicNotesAndClozeNotes()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new();
        article.Sections.Add(section);
        List<BasicNote> existingBasicNotes =
        [
            new() { Front = "Front", Back = "Back", OrdinalPosition = 0 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 2 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 4 }
        ];
        List<ClozeNote> existingClozeNotes =
        [
            new() { Text = "a", OrdinalPosition = 1},
            new() { Text = "b", OrdinalPosition = 3}
        ];
        
        section.BasicNotes = existingBasicNotes;
        section.ClozeNotes = existingClozeNotes;
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote basicNote = new()
        {
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 2,
            SectionId = section.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/BasicNotes", basicNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}