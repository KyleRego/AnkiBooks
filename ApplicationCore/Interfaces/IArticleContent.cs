namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleContent : IEntityBase, IOrderedElement
{
    public string? ArticleId { get; set; }
}