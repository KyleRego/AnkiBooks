using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Exceptions;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class SectionRepository(ApplicationDbContext dbContext)
            : OrderedElementRepositoryBase<Article, Section>(dbContext), ISectionRepository
{
    protected override Article GetParent(string parentId)
    {
        return _dbContext.Articles.First(article => article.Id == parentId);
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