using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class InsertOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task ClozeNoteIsInsertedInMiddleOfArticleWithNotes()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();

        ClozeNote clozeNote = new()
        {
            Text = "World2Cloze",
            OrdinalPosition = 6,
            SectionId = section.Id
        };

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.InsertOrderedElementAsync(clozeNote);

        ClozeNote updatedClozeNote = dbContext.ClozeNotes.First(cn => cn.Id == clozeNote.Id);
        Assert.Equal("World2Cloze", updatedClozeNote.Text);
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 11));
    }
}