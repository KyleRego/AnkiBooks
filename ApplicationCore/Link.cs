namespace AnkiBooks.ApplicationCore;

// TODO: Make Link a type of element
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