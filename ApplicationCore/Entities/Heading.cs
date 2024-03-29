using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.ApplicationCore.Entities;

// TODO: remove this in favor of markdown element that has headings?
public class Heading : ArticleElementBase
{
    [Required]
    public string? Title { get; set; }

    [Required]
    public HeadingType Type { get; set; }
}