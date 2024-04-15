using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class UpdateOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task BasicNoteIsUpdated()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithOneBasicNote();

        BasicNote currentBasicNote = section.BasicNotes.First();
        
        BasicNote updateNote = new()
        {
            Id = currentBasicNote.Id,
            SectionId = currentBasicNote.SectionId,
            Front = "World2",
            Back = "Hello2"
        };

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateOrderedElementAsync(currentBasicNote, updateNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == currentBasicNote.Id);
        Assert.Equal("World2", updatedBasicNote.Front);
        Assert.Equal("Hello2", updatedBasicNote.Back);
    }

    [Fact]
    public async Task InvalidBasicNoteOrdinalPositionsThrowAnException()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        BasicNote currentNote = section.BasicNotes.First(bn => bn.OrdinalPosition == 2);  

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Id = currentNote.Id,
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = -1,
                SectionId = section.Id
            };
        
            await basicNoteRepository.UpdateOrderedElementAsync(currentNote, basicNote);
        });

        await Assert.ThrowsAsync<OrdinalPositionException>(async () => {
            BasicNote basicNote = new()
            {
                Id = currentNote.Id,
                Front = "World2",
                Back = "Hello2",
                OrdinalPosition = 10,
                SectionId = section.Id
            };

            await basicNoteRepository.UpdateOrderedElementAsync(currentNote, basicNote);
        });
    }

    [Fact]
    public async Task BasicNoteIsShiftedToHigherOrdinalPosition()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        BasicNote currentNote = section.BasicNotes.First(bn => bn.OrdinalPosition == 2);

        BasicNote basicNote = new()
        {
            Id = currentNote.Id,
            Front = "World33",
            Back = "Hello33",
            OrdinalPosition = 5,
            SectionId = section.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateOrderedElementAsync(currentNote, basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World33", updatedBasicNote.Front);
        Assert.Equal("Hello33", updatedBasicNote.Back);
        Assert.Equal(5, updatedBasicNote.OrdinalPosition);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 10));
    }

    [Fact]
    public async Task BasicNoteIsShiftedToLowerOrdinalPosition()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        BasicNote currentNote = section.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        BasicNote basicNote = new()
        {
            Id = currentNote.Id,
            Front = "World334",
            Back = "Hello334",
            OrdinalPosition = 1,
            SectionId = section.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateOrderedElementAsync(currentNote, basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("World334", updatedBasicNote.Front);
        Assert.Equal("Hello334", updatedBasicNote.Back);
        Assert.Equal(1, updatedBasicNote.OrdinalPosition);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 10));
    }

    [Fact]
    public async Task FirstElementIsMovedToLastPosition()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        BasicNote currentNote = section.BasicNotes.First(bn => bn.OrdinalPosition == 0);

        BasicNote basicNote = new()
        {
            Id = currentNote.Id,
            Front = "4321",
            Back = "1234",
            OrdinalPosition = 9,
            SectionId = section.Id
        };
        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.UpdateOrderedElementAsync(currentNote, basicNote);

        BasicNote updatedBasicNote = dbContext.BasicNotes.First(bn => bn.Id == basicNote.Id);
        Assert.Equal("4321", updatedBasicNote.Front);
        Assert.Equal("1234", updatedBasicNote.Back);
        Assert.Equal(9, updatedBasicNote.OrdinalPosition);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 10));
    }
}