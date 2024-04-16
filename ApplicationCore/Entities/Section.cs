using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.ApplicationCore.Entities;

[Index(nameof(ArticleId), nameof(OrdinalPosition), IsUnique=true)]
public class Section : EntityBase, IOrdinalChild
{
    [Required]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public List<MarkdownContent> MarkdownContents { get; set; } = [];

    public List<IContent> OrderedContents()
    {
        return MarkdownContents.Cast<IContent>()
                .OrderBy(item => item.OrdinalPosition)
                .ToList();
    }

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];

    public List<INote> OrderedNotes()
    {
        return BasicNotes.Cast<INote>()
            .Concat(ClozeNotes.Cast<INote>())
            .OrderBy(item => item.OrdinalPosition)
            .ToList();
    }
}