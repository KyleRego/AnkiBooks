using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AnkiBooks.ApplicationCore.Identity;

namespace AnkiBooks.ApplicationCore.Entities;

public class Deck : ArticleElement
{
    public string Description { get; set; } = "New deck";

    public List<BasicNote> BasicNotes { get; set; } = [];

    public List<ClozeNote> ClozeNotes { get; set; } = [];
}