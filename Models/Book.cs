using AnkiBooks.Models.Identity;

namespace AnkiBooks.Models;

public class Book(string title) : PrimaryKeyIdBase
{
    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    [Required]
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }

    public ICollection<Article> Articles { get; } = [];
}