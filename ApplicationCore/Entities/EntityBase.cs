using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.ApplicationCore.Entities;

public abstract class EntityBase : IEntityBase
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}