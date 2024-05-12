using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public abstract class ArticleElement : EntityBase, IArticleElement, IOrdinalChild
{
    [Required]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }

    public string DomId()
    {
        return $"article-{ArticleId}-element-{OrdinalPosition}";
    }
}