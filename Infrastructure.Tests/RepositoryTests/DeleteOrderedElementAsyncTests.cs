using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task DeckInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var dbContext = InMemoryDbContext();

        Article article = await dbContext.CreateArticle(10);
        Deck deck = article.Decks.First(bn => bn.OrdinalPosition == 4);

        DeckRepository repository = new(dbContext);

        await repository.DeleteOrderedElementAsync(deck);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(d => d.Id == deck.Id));

        Assert.True(ValidateArticleOrdinalPositions(dbContext, article, 9));
    }
}