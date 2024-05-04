using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.DeckRepositoryTests;

public class UpdateOrderedElementAsyncTests : RepositoryTestBase
{
    // TODO: This test could be elsewhere since ordinal positions are not involved
    [Fact]
    public async Task DeckIsUpdated()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(1);

        Deck deck = article.Decks.First();

        Deck editDeck = new()
        {
            Id = deck.Id,
            ArticleId = deck.ArticleId,
            Description = "World20"
        };

        DeckRepository deckRepository = new(dbContext);

        await deckRepository.UpdateOrderedElementAsync(deck, editDeck);

        Deck updatedDeck = dbContext.Decks.First(d => d.Id == deck.Id);
        Assert.Equal("World20", updatedDeck.Description);
    }

    [Fact]
    public async Task InvalidOrdinalPositionUpdateThrowsException()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);
        Deck deck = article.Decks.First(d => d.OrdinalPosition == 2);  

        DeckRepository repository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            Deck editDeck = new()
            {
                Id = deck.Id,
                OrdinalPosition = -1,
                ArticleId = article.Id
            };
        
            await repository.UpdateOrderedElementAsync(deck, editDeck);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            Deck editDeck = new()
            {
                Id = deck.Id,
                OrdinalPosition = 10,
                ArticleId = article.Id
            };

            await repository.UpdateOrderedElementAsync(deck, editDeck);
        });
    }

    [Fact]
    public async Task ShiftingToHigherOrdinalPositionRearrangesOthers()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);
        Deck deck = article.Decks.First(bn => bn.OrdinalPosition == 2);

        Deck editDeck = new()
        {
            Id = deck.Id,
            OrdinalPosition = 5,
            ArticleId = article.Id
        };
        DeckRepository repository = new(dbContext);

        await repository.UpdateOrderedElementAsync(deck, editDeck);

        Deck updatedDeck = dbContext.Decks.First(bn => bn.Id == editDeck.Id);

        Assert.Equal(5, updatedDeck.OrdinalPosition);
        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 10));
    }

    [Fact]
    public async Task ShiftingToLowerOrdinalPositionWorks()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);
        Deck deck = article.Decks.First(bn => bn.OrdinalPosition == 4);

        Deck editDeck = new()
        {
            Id = deck.Id,
            OrdinalPosition = 1,
            ArticleId = article.Id
        };
        DeckRepository repository = new(dbContext);

        await repository.UpdateOrderedElementAsync(deck, editDeck);

        Deck updatedDeck = dbContext.Decks.First(bn => bn.Id == editDeck.Id);
        Assert.Equal(1, updatedDeck.OrdinalPosition);
        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 10));
    }

    [Fact]
    public async Task ShiftingFromFirstToLastPositionWorks()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);
        Deck deck = article.Decks.First(bn => bn.OrdinalPosition == 0);

        Deck editDeck = new()
        {
            Id = deck.Id,
            OrdinalPosition = 9,
            ArticleId = article.Id
        };
        DeckRepository repository = new(dbContext);

        await repository.UpdateOrderedElementAsync(deck, editDeck);

        Deck updatedDeck = dbContext.Decks.First(bn => bn.Id == editDeck.Id);
        Assert.Equal(9, updatedDeck.OrdinalPosition);
        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 10));
    }
}