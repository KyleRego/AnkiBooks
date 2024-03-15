using System.ComponentModel.DataAnnotations;

namespace Backend.Database.Models;

public class Link
{
    [Key]
    public long LinkId { get; set; }

    [Url]
    [Required]
    public string? URL { get; set; }

    [Required]
    public bool IsComplete { get; set; }
}