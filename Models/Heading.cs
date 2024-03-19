namespace AnkiBooks.Models;

public class Heading : ArticleElement
{
    [Required]
    public string? Title { get; set; }
}