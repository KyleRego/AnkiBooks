using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public class Book : EntityBase, IPublicViewable
{
    public bool PublicViewable { get; set; } = false;

    public string Title { get; set; } = "New book";

    [ForeignKey(nameof(User))]
    public string? UserId { get; set; }

    [JsonIgnore]
    public ApplicationUser? User { get; set; }
}

internal interface IPublicViewable
{
}