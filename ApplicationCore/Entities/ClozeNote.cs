using System.Text.RegularExpressions;

namespace AnkiBooks.ApplicationCore.Entities;

public partial class ClozeNote : Card
{
    [Required]
    public string Text { get; set; } = "";

    private readonly string clozeMarkersRegex = "{{c\\d::(.*?)}}";
    private readonly string clozeMarkersReplacement = "[...]";

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
        return Regex.Replace(Text, clozeMarkersRegex, $"<span class=\"cloze-question\">{clozeMarkersReplacement}</span>");
    }

    public string ClozeBackText()
    {
        return Regex.Replace(Text, clozeMarkersRegex, "$1");
    }

    public string ClozeBackHtml()
    {
        return Regex.Replace(Text, clozeMarkersRegex, "<span class=\"cloze-answer\">$1</span>");     
    }
}