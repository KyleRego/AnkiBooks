using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.ApplicationCore;

public class Heading : ArticleElement
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public HeadingType Type { get; set; }
}