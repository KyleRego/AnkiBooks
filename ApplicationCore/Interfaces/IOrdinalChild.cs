namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IOrdinalChild : IEntityBase
{
    public int OrdinalPosition { get; set; }
}