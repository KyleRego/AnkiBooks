using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

// TODO: Rename this to ArticleText or something
public class MarkdownContent : ArticleElement
{
    [Required]
    public string? Text { get; set; }

    public override string ElementName()
    {
        return Text?.Split("\n")[0] ?? "Empty markdown";
    }
}