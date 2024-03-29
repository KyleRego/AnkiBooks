namespace AnkiBooks.ApplicationCore;

public class ClozeNote : ArticleElement, IHasConcepts
{
    [Required]
    public string? Text { get; set; }

    public ICollection<Concept> Concepts { get; } = [];
}