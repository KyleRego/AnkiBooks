using AnkiBooks.Models.Enums;

namespace AnkiBooks.Models;

public class Heading : ArticleElement
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public HeadingType Type { get; set; }
}