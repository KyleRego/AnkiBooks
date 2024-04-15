using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.ApplicationCore.Entities;

[Index(nameof(ArticleId), nameof(OrdinalPosition), IsUnique=true)]
public class Section(string title) : EntityBase, IOrderedElement, IOrderedElementsParent
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];

    public string ParentId()
    {
        return ArticleId!;
    }

    public List<IOrderedElement> OrderedElements()
    {
        return BasicNotes.Cast<IOrderedElement>()
            .Concat(ClozeNotes.Cast<IOrderedElement>())
            .OrderBy(item => item.OrdinalPosition)
            .ToList();
    }

    [Required]
    public string? Text { get; set; }
}