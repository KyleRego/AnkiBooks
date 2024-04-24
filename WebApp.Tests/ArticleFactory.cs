using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.WebApp.Tests;

public class ArticleFactory
{
    public static async Task<Article> ArticleWithTenAlternatingBasicAndClozeNotes(TestServerFactory<Program> _factory)
    {
        using IServiceScope scope = _factory.Services.CreateScope();
        ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Article article = new("Test article")
        {
            BasicNotes =
            [
                new() { Front = "basic0", Back = "b", OrdinalPosition = 0 },
                new() { Front = "basic2", Back = "b", OrdinalPosition = 2 },
                new() { Front = "basic4", Back = "b", OrdinalPosition = 4 },
                new() { Front = "basic6", Back = "b", OrdinalPosition = 6 },
                new() { Front = "basic8", Back = "b", OrdinalPosition = 8 },
            ],
            ClozeNotes =
            [
                new() { Text = "cloze1", OrdinalPosition = 1 },
                new() { Text = "cloze3", OrdinalPosition = 3 },
                new() { Text = "cloze5", OrdinalPosition = 5 },
                new() { Text = "cloze7", OrdinalPosition = 7 },
                new() { Text = "cloze9", OrdinalPosition = 9 },
            ]
        };

        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();

        return article;
    }
}