using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.Infrastructure.Tests.Extensions;

public static class CreateArticleExtension
{
    public async static Task<Article> CreateArticle(this ApplicationDbContext dbContext)
    {
        Article article = new();

        dbContext.Articles.Add(article);

        await dbContext.SaveChangesAsync();

        return article;
    }

    public async static Task<Article> CreateArticle(this ApplicationDbContext dbContext, int numberOfDecks)
    {
        Article article = new();

        for (int i = 0; i < numberOfDecks; i++)
        {
            Deck deck = new()
            {
                OrdinalPosition = i
            };

            article.Decks.Add(deck);
        }

        dbContext.Articles.Add(article);

        await dbContext.SaveChangesAsync();

        return article;
    }
}
