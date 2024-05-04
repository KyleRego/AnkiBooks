using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.DeckRepositoryTests;

public class InsertOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task InvalidOrdinalPositionsInsertThrowsException()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);

        DeckRepository deckRepository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            Deck deck = new()
            {
                OrdinalPosition = -1,
                ArticleId = article.Id
            };

            await deckRepository.InsertOrderedElementAsync(deck);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            Deck deck = new()
            {
                OrdinalPosition = 11,
                ArticleId = article.Id
            };

            await deckRepository.InsertOrderedElementAsync(deck);
        });
    }

    [Fact]
    public async Task InsertDeckAtBeginningWorks()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);

        Deck deck = new()
        {
            Description = "World2",
            OrdinalPosition = 0,
            ArticleId = article.Id
        };
        
        DeckRepository deckRepository = new(dbContext);

        await deckRepository.InsertOrderedElementAsync(deck);

        Deck insertedDeck = dbContext.Decks.First(d => d.Id == deck.Id);

        Assert.Equal("World2", insertedDeck.Description);

        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 11));
    }

    [Fact]
    public async Task InsertDeckAtEndWorks()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);

        Deck deck = new()
        {
            Description = "Hello2",
            OrdinalPosition = 10,
            ArticleId = article.Id
        };
        
        DeckRepository repository = new(dbContext);

        await repository.InsertOrderedElementAsync(deck);

        Deck insertedDeck = dbContext.Decks.First(d => d.Id == deck.Id);

        Assert.Equal("Hello2", deck.Description);
        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 11));
    }

    [Fact]
    public async Task InsertDeckInMiddleWorks()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);

        Deck deck = new()
        {
            Description = "World5",
            OrdinalPosition = 3,
            ArticleId = article.Id
        };

        DeckRepository deckRepository = new(dbContext);

        await deckRepository.InsertOrderedElementAsync(deck);

        Deck updatedDeck = dbContext.Decks.First(d => d.Id == deck.Id);

        Assert.Equal("World5", updatedDeck.Description);
        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 11));
    }
}