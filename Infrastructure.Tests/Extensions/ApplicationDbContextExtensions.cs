using System.Runtime.CompilerServices;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.Infrastructure.Tests.Extensions;

public static class ApplicationDbContextExtensions
{
    public static async Task<Section> CreateSectionWithOneBasicNote(this ApplicationDbContext dbContext)
    {
        Article article = new("Test article");
        dbContext.Articles.Add(article);
        Section section = new("Test section with one basic note")
        {
            BasicNotes = [ new() { Front = "Basic note front", Back = "Basic note back" } ]
        };
        article.Sections.Add(section);
        await dbContext.SaveChangesAsync();
        return section;
    }

    public static async Task<Section> CreateSectionWithTenAlternatingBasicAndClozeNotes(this ApplicationDbContext dbContext)
    {
        Article article = new("Test article");
        dbContext.Articles.Add(article);
        Section section = new("Test section")
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

        article.Sections.Add(section);
        await dbContext.SaveChangesAsync();

        return section;
    }
}