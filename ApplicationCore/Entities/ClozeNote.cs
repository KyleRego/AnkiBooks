using System.Text.RegularExpressions;
using Ganss.Xss;

namespace AnkiBooks.ApplicationCore.Entities;

public partial class ClozeNote : Card
{
    [Required]
    public string Text { get; set; } = "";

    private readonly string clozeMarkersRegex = "{{c\\d::(.*?)}}";
    private readonly string clozeMarkersReplacement = "[...]";
    private readonly HtmlSanitizer sanitizer = new();

    public ClozeNote()
    {
        sanitizer.AllowedAttributes.Add("class");
    }

    public bool ValidCloze()
    {
        if (string.IsNullOrWhiteSpace(Text)) return false;

        return Regex.IsMatch(Text, clozeMarkersRegex);
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