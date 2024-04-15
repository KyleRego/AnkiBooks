using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public class ClozeNote : NoteBase, INote
{
    [Required]
    public string? Text { get; set; }
}