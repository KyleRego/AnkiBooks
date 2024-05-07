namespace AnkiBooks.ApplicationCore.Entities;

public class Link : InfoSource
{
    public string? Name { get; set; }

    [Required]
    public string Url { get; set; } = null!;
}