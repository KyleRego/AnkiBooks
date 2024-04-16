using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnkiBooks.ApplicationCore.Entities;

[Index(nameof(SectionId), nameof(OrdinalPosition), IsUnique=true)]
public abstract class ContentBase : EntityBase, IContent, IOrdinalChild
{
    [Required]
    public string? SectionId { get; set; }

    [JsonIgnore]
    [ForeignKey("SectionId")]
    public Section? Section { get; set; }

    [Required]
    public int OrdinalPosition { get; set; }
}