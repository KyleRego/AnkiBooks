using AnkiBooks.Models.Identity;

namespace AnkiBooks.Models;

public class Concept : PrimaryKeyIdBase
{
    [Required]
    public ICollection<ConceptName> Names { get; set; } = [];

    [Required]
    public bool Public { get; set; } = false;

    public ICollection<ArticleElement> Elements { get; } = [];

    [Required]
    public string? ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
}

public class ConceptName
{
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Name { get; set; }

    [Required]
    public string? ConceptId { get; set; }
    public Concept? Concept { get; set; }
}