using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.Infrastructure.Tests.Extensions;
using AnkiBooks.Infrastructure.Tests.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Tests.RepositoryTests.BasicNoteRepositoryTests;

public class DeleteOrderedElementAsyncTests : RepositoryTestBase
{
    [Fact]
    public async Task BasicNoteInMiddleIsDeletedAndElementsAreShiftedDown()
    {
        using var dbContext = InMemoryDbContext();

        Section section = await dbContext.CreateSectionWithTenAlternatingBasicAndClozeNotes();
        BasicNote noteToDelete = section.BasicNotes.First(bn => bn.OrdinalPosition == 4);

        BasicNoteRepository basicNoteRepository = new(dbContext);

        await basicNoteRepository.DeleteOrderedElementAsync(noteToDelete);

        Assert.Null(dbContext.BasicNotes.FirstOrDefault(bn => bn.Id == noteToDelete.Id));
        Assert.True(SectionValidator.CorrectElementsCountAndOrdinalPositions(dbContext, section, 9));
    }
}