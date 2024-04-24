using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Identity;

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

    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }

    public List<Article> ChildArticles { get; set; } = [];
    public List<BasicNote> BasicNotes { get; set; } = [];
    public List<ClozeNote> ClozeNotes { get; set; } = [];
    public List<MarkdownContent> MarkdownContents { get; set; } = [];

    public List<ArticleElement> OrderedElements()
    {
        return BasicNotes.Cast<ArticleElement>()
            .Concat(ClozeNotes.Cast<ArticleElement>())
            .Concat(MarkdownContents.Cast<ArticleElement>())
            .OrderBy(item => item.OrdinalPosition)
            .ToList();
    }
}