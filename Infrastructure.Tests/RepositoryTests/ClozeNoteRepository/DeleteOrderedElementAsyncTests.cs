using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.ClozeNoteRepositoryTests;

public class DeleteOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        ClozeNote noteToDelete = section.ClozeNotes.First(cn => cn.OrdinalPosition == 3);

        ClozeNoteRepository clozeNoteRepository = new(dbContext);

        await clozeNoteRepository.DeleteOrderedElementAsync(noteToDelete);

        Assert.Null(dbContext.ClozeNotes.FirstOrDefault(cn => cn.Id == noteToDelete.Id));
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 9));
    }
}