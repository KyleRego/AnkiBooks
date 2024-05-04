using System.Linq.Expressions;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AnkiBooks.Infrastructure.Repository;

public class ArticleRepository(ApplicationDbContext dbContext) : IArticleRepository
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<List<Article>> GetArticlesAsync()
    {
        // TODO: Look into a recursive way of doing this
        return await _dbContext.Articles
                            .Include(a => a.ChildArticles)
                            .ThenInclude(ca => ca.ChildArticles)
                            .ThenInclude(cca => cca.ChildArticles)
                            .Where(a => a.ParentArticleId == null)
                            .ToListAsync();
    }

    public async Task<Article?> GetArticleAsync(string articleId)
    {
        return await _dbContext.Articles
                    .Include(a => a.Decks)
                    .Include(a => a.MarkdownContents)
                    .FirstOrDefaultAsync(a => a.Id == articleId);
    }

    public async Task<Article> InsertArticleAsync(Article article)
    {
        _dbContext.Articles.Add(article);
        await _dbContext.SaveChangesAsync();
        return article;
    }

    public async Task DeleteArticleAsync(Article article)
    {
        _dbContext.Articles.Remove(article);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Article> UpdateArticleAsync(Article article)
    {
        _dbContext.Entry(article).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return article;
    }

    public async Task<bool> ArticleExists(string articleId)
    {
        return await _dbContext.Articles.AnyAsync(a => a.Id == articleId);
    }
}