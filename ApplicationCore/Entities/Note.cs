namespace AnkiBooks.ApplicationCore.Entities;

public class Note : EntityBase
{
    public Note(NoteType noteType)
    {
        FieldValues = new();

        foreach (NoteField field in noteType.Fields)
        {
            FieldValues[field.Name] = "";
        }
    }

    [Required]
    public string? NoteTypeId { get; set; }

    public NoteType? NoteType { get; set; }

    public Dictionary<string, string> FieldValues { get; set; }
}