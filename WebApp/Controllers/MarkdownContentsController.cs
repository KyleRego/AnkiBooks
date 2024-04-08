using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkdownContentsController(IMarkdownContentRepository markdownContentRepository,
                                        ILogger<MarkdownContentsController> logger) : ControllerBase
{
    private readonly IMarkdownContentRepository _markdownContentRepository = markdownContentRepository;
    private readonly ILogger<MarkdownContentsController> _logger = logger;

    // GET: api/MarkdownContents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarkdownContent>>> GetMarkdownContents()
    {
        return await _markdownContentRepository.GetMarkdownContentsAsync();
    }

    // GET: api/MarkdownContents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MarkdownContent>> GetMarkdownContent(string id)
    {
        MarkdownContent? markdownContent = await _markdownContentRepository.GetMarkdownContentAsync(id);

        if (markdownContent == null)
        {
            return NotFound();
        }

        return markdownContent;
    }

    // PUT: api/MarkdownContents/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<MarkdownContent>> PutMarkdownContent(string id, MarkdownContent markdownContent)
    {
        if (id != markdownContent.Id)
        {
            return BadRequest();
        }

        try
        {
            return (MarkdownContent)await _markdownContentRepository.UpdateArticleElementAsync(markdownContent);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await MarkdownContentExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: api/MarkdownContents
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<MarkdownContent>> PostMarkdownContent(MarkdownContent markdownContent)
    {
        try
        {
            await _markdownContentRepository.InsertArticleElementAsync(markdownContent);
        }
        catch (DbUpdateException)
        {
            if (await MarkdownContentExists(markdownContent.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetMarkdownContent", new { id = markdownContent.Id }, markdownContent);
    }

    // DELETE: api/MarkdownContents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarkdownContent(string id)
    {
        MarkdownContent? markdownContent = await _markdownContentRepository.GetMarkdownContentAsync(id);
        if (markdownContent == null)
        {
            return NotFound();
        }

        await _markdownContentRepository.DeleteArticleElementAsync(markdownContent);

        return NoContent();
    }

    private async Task<bool> MarkdownContentExists(string id)
    {
        return await _markdownContentRepository.MarkdownContentExists(id);
    }
}
