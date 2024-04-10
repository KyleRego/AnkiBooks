using System.ComponentModel.DataAnnotations.Schema;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class Article(string title) : EntityBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    public string? ParentArticleId { get; set; }
    public Article? ParentArticle { get; set; }

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];

    public List<MarkdownContent> MarkdownContents { get; set; } = [];

    public List<IArticleElement> OrderedElements()
    {
        return BasicNotes.Cast<IArticleElement>()
            .Concat(ClozeNotes.Cast<IArticleElement>())
            .Concat(MarkdownContents.Cast<IArticleElement>())
            .OrderBy(item => item.OrdinalPosition)
            .ToList();
    }
}