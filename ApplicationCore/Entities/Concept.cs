using AnkiBooks.ApplicationCore.Identity;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

// TODO: Add aggregates folder, refactor this
public class Concept : EntityBase
{
    [Required]
    public ICollection<ConceptName> Names { get; set; } = [];

    [Required]
    public bool Public { get; set; } = false;

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