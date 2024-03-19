namespace AnkiBooks.Models;

public class BasicNote : ArticleElement, IHasConcepts
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}