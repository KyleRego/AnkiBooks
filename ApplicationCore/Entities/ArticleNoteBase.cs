using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

[Index(nameof(ArticleId), nameof(OrdinalPosition), IsUnique=true)]
public abstract class ArticleNoteBase : EntityBase, IArticleNote
{
    [Required]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}