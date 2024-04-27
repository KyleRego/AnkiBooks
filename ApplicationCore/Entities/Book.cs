namespace AnkiBooks.ApplicationCore.Entities;

public class Book : EntityBase, IPublicViewable
{
    public bool PublicViewable { get; set; } = false;

    public string Title { get; set; } = "New book";
}

internal interface IPublicViewable
{
}