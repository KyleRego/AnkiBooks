using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public abstract class NoteRepositoryBase<T>(ApplicationDbContext dbContext)
    : OrderedElementRepositoryBase<Section, T>(dbContext) where T : INote
{
    protected override Section GetParent(string parentId)
    {
        // TODO: This query is going to end up repeated; it can be put one place later
        return _dbContext.Sections
                        .Include(article => article.BasicNotes)
                        .Include(article => article.ClozeNotes)
                        .First(article => article.Id == parentId);
    }
}