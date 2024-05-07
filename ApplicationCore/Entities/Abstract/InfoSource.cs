using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public abstract class InfoSource : EntityBase
{
    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }

    [ForeignKey(nameof(Article))]
    public string? ArticleId { get; set; }

    [JsonIgnore]
    public Article? Article { get; set; }

    public bool Complete { get; set; }
}