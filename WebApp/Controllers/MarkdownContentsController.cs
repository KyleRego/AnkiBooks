using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarkdownContentsController(IMarkdownContentRepository mdContentRepository) : ControllerBase
{
    private readonly IMarkdownContentRepository _mdContentRepository = mdContentRepository;

    // GET: api/MarkdownContents
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MarkdownContent>>> GetMarkdownContents()
    {
        return await _mdContentRepository.GetMarkdownContentsAsync();
    }

    // GET: api/MarkdownContents/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MarkdownContent>> GetMarkdownContent(string id)
    {
        MarkdownContent? mdContent = await _mdContentRepository.GetMarkdownContentAsync(id);

        if (mdContent == null)
        {
            return NotFound();
        }

        return mdContent;
    }

    // PUT: api/MarkdownContents/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<MarkdownContent>> PutMarkdownContent(string id, MarkdownContent mdContent)
    {
        if (id != mdContent.Id)
        {
            return BadRequest();
        }

        try
        {
            return await _mdContentRepository.UpdateAsync(mdContent);
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
    public async Task<ActionResult<MarkdownContent>> PostMarkdownContent(MarkdownContent mdContent)
    {
        try
        {
            await _mdContentRepository.InsertAsync(mdContent);
        }
        catch (DbUpdateException)
        {
            if (await MarkdownContentExists(mdContent.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetMarkdownContent", new { id = mdContent.Id }, mdContent);
    }

    // DELETE: api/MarkdownContents/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarkdownContent(string id)
    {
        MarkdownContent? mdContent = await _mdContentRepository.GetMarkdownContentAsync(id);
        if (mdContent == null)
        {
            return NotFound();
        }

        await _mdContentRepository.DeleteAsync(mdContent);

        return NoContent();
    }

    private async Task<bool> MarkdownContentExists(string id)
    {
        return await _mdContentRepository.MarkdownContentExists(id);
    }
}
