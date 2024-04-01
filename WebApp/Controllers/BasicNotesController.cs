using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasicNotesController(IBasicNoteRepository basicNoteRepository) : ControllerBase
{
    private readonly IBasicNoteRepository _basicNoteRepository = basicNoteRepository;

    // GET: api/BasicNotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BasicNote>>> GetBasicNotes()
    {
        return await _basicNoteRepository.GetBasicNotesAsync();
    }

    // GET: api/BasicNotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<BasicNote>> GetBasicNote(string id)
    {
        BasicNote? basicNote = await _basicNoteRepository.GetBasicNoteAsync(id);

        if (basicNote == null)
        {
            return NotFound();
        }

        return basicNote;
    }

    // PUT: api/BasicNotes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBasicNote(string id, BasicNote basicNote)
    {
        if (id != basicNote.Id)
        {
            return BadRequest();
        }

        try
        {
            await _basicNoteRepository.UpdateArticleElementAsync(basicNote);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await BasicNoteExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok();
    }

    // POST: api/BasicNotes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BasicNote>> PostBasicNote(BasicNote basicNote)
    {
        try
        {
            await _basicNoteRepository.InsertArticleElementAsync(basicNote);
        }
        catch (DbUpdateException)
        {
            if (await BasicNoteExists(basicNote.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetBasicNote", new { id = basicNote.Id }, basicNote);
    }

    // DELETE: api/BasicNotes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBasicNote(string id)
    {
        BasicNote? basicNote = await _basicNoteRepository.GetBasicNoteAsync(id);
        if (basicNote == null)
        {
            return NotFound();
        }

        await _basicNoteRepository.DeleteArticleElementAsync(basicNote);

        return NoContent();
    }

    private async Task<bool> BasicNoteExists(string id)
    {
        return await _basicNoteRepository.BasicNoteExists(id);
    }
}
