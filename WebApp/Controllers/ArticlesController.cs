using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using Microsoft.AspNetCore.Authorization;
using AnkiBooks.WebApp.Policies.Requirements;
using System.Security.Claims;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
public class ArticlesController(IArticleRepository repository,
                                IAuthorizationService authorizationService,
                                ILogger<ArticlesController> logger) : ApplicationController
{
    private readonly IArticleRepository _repository = repository;
    private readonly IAuthorizationService _authorizationService = authorizationService;
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
        ClaimsPrincipal user = HttpContext.User;
        if (user == null) return new ForbidResult();

        Article? article = await _repository.GetArticleAsync(articleId);
        if (article == null) return NotFound();

        AuthorizationResult authorizationResult = await _authorizationService.AuthorizeAsync(HttpContext.User, article, "ArticlePolicy");

        if (authorizationResult.Succeeded)
        {
            logger.LogInformation("Article authorization was successful");
            return article;
        }
        else
        {
            logger.LogInformation("Article authorization failed");
            return new ForbidResult();
        }
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

        // TODO: Need to look into policy-based authorization or something

        await _repository.DeleteArticleAsync(article);

        return NoContent();
    }

    private async Task<bool> ArticleExists(string articleId)
    {
        return await _repository.ArticleExists(articleId);
    }
}