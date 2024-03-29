namespace AnkiBooks.ApplicationCore.Entities;

public abstract class EntityBase
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}