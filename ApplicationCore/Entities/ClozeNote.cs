using System.Text.RegularExpressions;
using Ganss.Xss;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : Card
{
    [Required]
    [RegularExpression($".*{clozeMarkersRegex}.*", ErrorMessage = "Text must have at least one {{{{c1::cloze test}}}}.")]
    public string Text { get; set; } = "";

    private const string clozeMarkersRegex = "{{c\\d::(.*?)}}";
    private const string clozeMarkersReplacement = "[...]";
    private readonly HtmlSanitizer sanitizer = new();

    public ClozeNote()
    {
        sanitizer.AllowedAttributes.Add("class");
    }

    public string ClozeFrontText()
    {
        return Regex.Replace(Text, clozeMarkersRegex, clozeMarkersReplacement);
    }

    public string ClozeFrontHtml()
    {
        string sanitizedText = sanitizer.Sanitize(Text);
        return Regex.Replace(sanitizedText, clozeMarkersRegex, $"<span class=\"cloze-question\">{clozeMarkersReplacement}</span>");
    }

    public string ClozeBackText()
    {
        return Regex.Replace(Text, clozeMarkersRegex, "$1");
    }

    public string ClozeBackHtml()
    {
        string sanitizedText = sanitizer.Sanitize(Text);
        return Regex.Replace(sanitizedText, clozeMarkersRegex, "<span class=\"cloze-answer\">$1</span>");     
    }
}