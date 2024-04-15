using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.ApplicationCore.Interfaces;

public interface ISectionRepository : IOrderedElementRepository<Section>
{
    Task<List<Section>> GetSectionsAsync();
    Task<Section?> GetSectionAsync(string sectionId);
    Task<bool> SectionExists(string sectionId);
}
