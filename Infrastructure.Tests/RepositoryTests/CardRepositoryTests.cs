using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Enums;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests;

public class CardRepositoryTests : RepositoryTestBase
{
    [Fact]
    public async Task GetSuccessfulRepetitionsStreakReturnsZeroWhenOnlyOneFailureRep()
    {
        using var dbContext = InMemoryDbContext();

        Card card = await dbContext.CreateCard();
        Repetition rep = new() { Grade = Grade.Bad, CardId = card.Id };
        card.Repetitions.Add(rep);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        CardRepository repository = new(dbContext);
        int result = await repository.GetSuccessfulRepetitionsStreak(card);

        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetSuccessfulRepetitionsStreakReturnsOneWhenOnlyOneSuccessfulRep()
    {
        using var dbContext = InMemoryDbContext();

        Card card = await dbContext.CreateCard();
        Repetition rep = new() { Grade = Grade.Good, CardId = card.Id };
        card.Repetitions.Add(rep);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        CardRepository repository = new(dbContext);
        int result = await repository.GetSuccessfulRepetitionsStreak(card);

        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetSuccessfulRepetitionsStreakCorrectlyReturnsThree()
    {
        using var dbContext = InMemoryDbContext();

        Card card = await dbContext.CreateCard();
        Repetition rep1 = new() { Grade = Grade.Good, CardId = card.Id, OccurredAt = 100 };
        Repetition rep2 = new() { Grade = Grade.Bad, CardId = card.Id, OccurredAt = 200 };
        Repetition rep3 = new() { Grade = Grade.Good, CardId = card.Id, OccurredAt = 300 };
        Repetition rep4 = new() { Grade = Grade.Good, CardId = card.Id, OccurredAt = 400 };
        Repetition rep5 = new() { Grade = Grade.Good, CardId = card.Id, OccurredAt = 500 };

        card.Repetitions.AddRange([rep1, rep2, rep3, rep4, rep5]);

        await dbContext.SaveChangesAsync();

        dbContext.ChangeTracker.Clear();

        CardRepository repository = new(dbContext);
        int result = await repository.GetSuccessfulRepetitionsStreak(card);

        Assert.Equal(3, result);
    }
}