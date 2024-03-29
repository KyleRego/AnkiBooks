namespace AnkiBooks.ApplicationCore.Entities;

public class BasicNote : ArticleElementBase, IHasConcepts
{
    [Required]
    public string? Front { get; set; }

    [Required]
    public string? Back { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}