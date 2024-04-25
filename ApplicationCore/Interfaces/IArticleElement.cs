namespace AnkiBooks.ApplicationCore.Interfaces;

public interface IArticleElement : IEntityBase, IOrdinalChild
{
    public string? ArticleId { get; set; }
}

public interface IContent : IArticleElement
{

}

public interface INote : IArticleElement
{
    
}