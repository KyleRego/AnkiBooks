using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public class Deck : ArticleElement
{
    [Required]
    public string Name { get; set; } = "New deck";

    public string Description { get; set; } = "";

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];

    public List<Card> Cards()
    {
        return BasicNotes.Cast<Card>()
            .Concat(ClozeNotes.Cast<Card>())
            .ToList();
    }
}