namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElement : IEntityBase, IOrdinalChild
{
    public string? ArticleId { get; set; }
}
