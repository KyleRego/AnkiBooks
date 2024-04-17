using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class InsertOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task InvalidBasicNoteOrdinalPositionsThrowAnException()
    {
        using var dbContext = InMemoryDbContext();
        dbContext.Database.EnsureCreated();
        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = -1,
                SectionId = section.Id
            };
        
            await basicNoteRepository.InsertOrderedElementAsync(basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = 11,
                SectionId = section.Id
            };

            await basicNoteRepository.InsertOrderedElementAsync(basicNote);
        });
    }

    [Fact]
    public async Task BasicNoteIsInsertedAtBeginningOfArticleWithNotes()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 0,
            SectionId = section.Id
        };
        
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertOrderedElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 11));
    }

    [Fact]
    public async Task BasicNoteIsInsertedAtEndOfArticleWithNotes()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 10,
            SectionId = section.Id
        };
        
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertOrderedElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 11));
    }

    [Fact]
    public async Task BasicNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();

        BasicNote basicNote = new()
        {
            Front = "World2",
            Back = "Hello2",
            OrdinalPosition = 3,
            SectionId = section.Id
        };

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.InsertOrderedElementAsync(basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 11));
    }
}