using System.ComponentModel.DataAnnotations.Schema;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class Article(string title) : EntityBase, IOrderedElementsParent
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? ParentArticleId { get; set; }
    public Article? ParentArticle { get; set; }

    public List<Section> Sections { get; set; } = [];

    public List<IOrderedElement> OrderedElements()
    {
        return Sections.Cast<IOrderedElement>()
            .OrderBy(item => item.OrdinalPosition)
            .ToList();
    }
}