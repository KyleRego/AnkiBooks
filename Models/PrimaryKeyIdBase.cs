namespace AnkiBooks.Models;

public abstract class PrimaryKeyIdBase
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}