using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.Models.Interfaces;

namespace AnkiBooks.Models;

[Index(nameof(ArticleId), nameof(OrdinalPosition), IsUnique=true)]
public abstract class ArticleElement : PrimaryKeyIdBase, IArticleElement
{
    [Required]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }
}