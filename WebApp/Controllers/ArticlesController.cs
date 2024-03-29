using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ArticlesController(IArticleRepository articleRepository) : ControllerBase
{
    private readonly IArticleRepository _articleRepository = articleRepository;

    // GET: api/Articles
    [HttpGet]
    public async IAsyncEnumerable<IEnumerable<Article>> GetArticles()
    {
        List<Article> articles = await _articleRepository.GetArticlesAsync();
        yield return articles;
    }

    // GET: api/Articles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Article>> GetArticle(string id)
    {
        Article? article = await _articleRepository.GetArticleWithOrderedElementsAsync(id);

        if (article == null)
        {
            return NotFound();
        }

        return article;
    }

    // PUT: api/Articles/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutArticle(string id, Article article)
    {
        if (id != article.Id)
        {
            return BadRequest();
        }

        try
        {
            await _articleRepository.UpdateArticleAsync(article);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArticleExists(id))
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

    // POST: api/Articles
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Article>> PostArticle(Article article)
    {
        try
        {
            await _articleRepository.InsertArticleAsync(article);
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

        return CreatedAtAction("GetArticle", new { id = article.Id }, article);
    }

    // DELETE: api/Articles/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArticle(string id)
    {
        var article = await _articleRepository.GetArticleAsync(id);

        if (article == null)
        {
            return NotFound();
        }

        await _articleRepository.DeleteArticleAsync(article);

        return NoContent();
    }

    private async Task<bool> ArticleExists(string id)
    {
        return await _articleRepository.ArticleExists(id);
    }
}