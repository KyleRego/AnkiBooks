namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : ArticleElementBase, IHasConcepts
{
    [Required]
    public string? Text { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}