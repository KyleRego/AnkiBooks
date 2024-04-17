using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AnkiBooks.ApplicationCore.Entities;

public class Article(string title) : EntityBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    [ForeignKey(nameof(ParentArticle))]
    public string? ParentArticleId { get; set; }

    [JsonIgnore]
    public Article? ParentArticle { get; set; }

    public List<Article> ChildArticles { get; set; } = [];

    public List<Section> Sections { get; set; } = [];

    public List<Section> OrderedSections()
    {
        return Sections.OrderBy(item => item.OrdinalPosition).ToList();
    }
}