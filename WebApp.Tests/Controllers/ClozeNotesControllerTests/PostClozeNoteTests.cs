using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Mvc.Testing;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Tests.Controllers.ClozeNotesControllerTests;

public class PostClozeNoteTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task CreatesClozeNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new("Test section")
        {
            Text = "hello world"
        };
        article.Sections.Add(section);
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        ClozeNote clozeNote = new()
        {
            Text = "Content",
            OrdinalPosition = 0,
            SectionId = section.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/ClozeNotes", clozeNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task ShiftsOtherArticleElements()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new("Test section")
        {
            Text = "hello world"
        };
        article.Sections.Add(section);
        List<BasicNote> existingBasicNotes =
        [
            new() { Front = "Front", Back = "Back", OrdinalPosition = 0 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 1 },
            new() { Front = "Front", Back = "Back", OrdinalPosition = 2 }
        ];
        List<ClozeNote> existingClozeNotes = 
        [
            new() { Text = "Content", OrdinalPosition = 3 }
        ];
        
        section.BasicNotes = existingBasicNotes;
        section.ClozeNotes = existingClozeNotes;
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        ClozeNote clozeNote = new()
        {
            Text = "Content",
            OrdinalPosition = 1,
            SectionId = section.Id
        };

        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.PostAsJsonAsync("api/ClozeNotes", clozeNote);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}