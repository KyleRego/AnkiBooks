using AnkiBooks.Models.Identity;
using AnkiBooks.Models.Interfaces;

namespace AnkiBooks.Models;

public class Book(string title) : IPublishable
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    [Required]
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<Article> Articles { get; } = [];
}