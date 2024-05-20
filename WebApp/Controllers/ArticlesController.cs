using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
public class ArticlesController(IArticleRepository repository,
                                    ILogger<ArticlesController> logger) : ApplicationController
{
    private readonly IArticleRepository _repository = repository;
    private readonly ILogger<ArticlesController> _logger = logger;

    [HttpGet("api/UserArticles")]
    public async Task<ActionResult<IEnumerable<Article>>> GetUserArticles()
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        return await _repository.GetUserArticlesAsync(userId);
    }

    [HttpGet("api/Articles/{articleId}")]
    public async Task<ActionResult<Article>> GetArticle(string articleId)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        Article? article = await _repository.GetArticleAsync(articleId);
        if (article == null) return NotFound();

        return article;
    }

    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPatch("api/Articles/{articleId}")]
    public async Task<ActionResult<Article>> PatchArticle(string articleId, Article article)
    {
        if (articleId != article.Id) return BadRequest();
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        if (!await ArticleExists(articleId)) return NotFound();

        try
        {
            return await _repository.UpdateArticleAsync(article);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArticleExists(articleId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("api/Articles")]
    public async Task<ActionResult<Article>> PostArticle(Article article)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        try
        {
            await _repository.InsertArticleAsync(article);
        }
        catch (DbUpdateException)
        {
            if (await ArticleExists(article.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction(nameof(GetArticle), new { articleId = article.Id }, article);
    }

    [HttpDelete("api/Articles/{articleId}")]
    public async Task<IActionResult> DeleteArticle(string articleId)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        // TODO: Look into the eager loading here, and cascade deleting later
        Article? article = await _repository.GetArticleAsync(articleId);
        if (article == null) return NotFound();

        await _repository.DeleteArticleAsync(article);

        return NoContent();
    }

    private async Task<bool> ArticleExists(string articleId)
    {
        return await _repository.ArticleExists(articleId);
    }
}