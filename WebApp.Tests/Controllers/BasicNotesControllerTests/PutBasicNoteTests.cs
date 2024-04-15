using System.Net;
using System.Net.Http.Json;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Mvc.Testing;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Tests.Controllers.BasicNotesControllerTests;

public class PutBasicNoteTests(TestServerFactory<Program> factory) : IClassFixture<TestServerFactory<Program>>
{
    private readonly TestServerFactory<Program> _factory = factory;

    [Fact]
    public async Task UpdatesBasicNote()
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article");
        Section section = new("Test section")
        {
            Text = "hello world"
        };
        article.Sections.Add(section);
        BasicNote basicNote = new() { Front = "Hello", Back = "World", OrdinalPosition = 0 };
        section.BasicNotes.Add(basicNote);
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        BasicNote bnData = new()
        {
            Id = basicNote.Id,
            Front = "Front",
            Back = "Back",
            OrdinalPosition = 0,
            SectionId = section.Id
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