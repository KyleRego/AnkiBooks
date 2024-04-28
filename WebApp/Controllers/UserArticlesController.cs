using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Entities;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AnkiBooks.WebApp.Controllers;

[Authorize]
[Route("api/user/Articles")]
[ApiController]
public class UserArticlesController(IUserArticleRepository repository,
                                    ILogger<UserArticlesController> logger) : ControllerBase
{
    private readonly IUserArticleRepository _repository = repository;
    private readonly ILogger<UserArticlesController> _logger = logger;

    private string? CurrentUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    // GET: api/user/Articles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
    {
        _logger.LogCritical("made it here");
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");
        _logger.LogInformation("What the heck");

        List<Article> articles = await _repository.GetArticlesAsync(userId);
        _logger.LogInformation(articles.ToString());
        return articles;
    }

    // GET: api/user/Articles/5
    [HttpGet("{articleId}")]
    public async Task<ActionResult<Article>> GetArticle(string articleId)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        Article? article = await _repository.GetArticleAsync(userId, articleId);
        if (article == null) return NotFound();

        return article;
    }

    // PUT: api/user/Articles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{articleId}")]
    public async Task<IActionResult> PutArticle(string articleId, Article article)
    {
        if (articleId != article.Id) return BadRequest();
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        if (!await ArticleExists(userId, articleId)) return NotFound();

        try
        {
            await _repository.UpdateArticleAsync(article);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArticleExists(userId, articleId))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/user/Articles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Article>> PostArticle(Article article)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        try
        {
            await _repository.InsertArticleAsync(userId, article);
        }
        catch (DbUpdateException)
        {
            if (await ArticleExists(userId, article.Id))
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

    // DELETE: api/user/Articles/5
    [HttpDelete("{articleId}")]
    public async Task<IActionResult> DeleteArticle(string articleId)
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest("User ID not in claims");

        // TODO: Look into the eager loading here, and cascade deleting later
        Article? article = await _repository.GetArticleAsync(userId, articleId);
        if (article == null) return NotFound();

        await _repository.DeleteArticleAsync(article);

        return NoContent();
    }

    private async Task<bool> ArticleExists(string userId, string articleId)
    {
        return await _repository.ArticleExists(userId, articleId);
    }
}