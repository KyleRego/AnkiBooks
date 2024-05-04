using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.Infrastructure.Tests.Extensions;

public static class CreateDeckExtension
{
    public static async Task<Deck> CreateDeck(this ApplicationDbContext dbContext)
    {
        Article article = new();
        Deck deck = new();
        article.Decks.Add(deck);
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();
        return deck;
    }
}