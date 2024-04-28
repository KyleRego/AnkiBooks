using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public abstract class InfoSource : EntityBase
{
    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }

    public bool Complete { get; set; }
}