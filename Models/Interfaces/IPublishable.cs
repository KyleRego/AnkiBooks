namespace AnkiBooks.Models.Interfaces;

public interface IPublishable
{
    public bool Public { get; set; }
    
    // public ICollection<Suggestion> Suggestions { get; set; } = [];
}