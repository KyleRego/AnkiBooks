using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public class NoteType : EntityBase
{
    public required string Name { get; set; }

    public List<NoteField> Fields { get; set; } = [];

    public List<CardType> CardTypes { get; set; } = [];

    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }
}