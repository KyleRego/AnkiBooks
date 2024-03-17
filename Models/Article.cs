using AnkiBooks.Models.Interfaces;

namespace AnkiBooks.Models;

public class Article(string title) : IPublishable
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    public string Title { get; set; } = title;

    [Required]
    public bool Public { get; set; } = false;

    [Required]
    public string? BookId { get; set; }
    public Book? Book { get; set; }

    public ICollection<Element> Elements { get; } = [];

    public ICollection<Note> Notes { get; } = [];
}