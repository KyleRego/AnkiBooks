using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class SectionRepository(ApplicationDbContext dbContext)
            : OrderedElementRepositoryBase<Section>(dbContext), ISectionRepository
{
    protected override List<IOrdinalChild> GetAllOrdinalChildren(Section section)
    {
        return _dbContext.Sections.Where(
            sec => sec.ArticleId == section.ArticleId
        ).Cast<IOrdinalChild>().ToList();
    }

    protected override void AddElementToDbContext(Section section)
    {
        _dbContext.Sections.Add(section);
    }

    protected override void RemoveElementFromDbContext(Section section)
    {
        _dbContext.Sections.Remove(section);
    }

    public async Task<List<Section>> GetSectionsAsync()
    {
        return await _dbContext.Sections.ToListAsync();
    }

    public async Task<Section?> GetSectionAsync(string sectionId)
    {
        return await _dbContext.Sections.FirstOrDefaultAsync(bn => bn.Id == sectionId);
    }

    public async Task<bool> SectionExists(string markdownContentId)
    {
        return await _dbContext.Sections.AnyAsync(a => a.Id == markdownContentId);
    }
}