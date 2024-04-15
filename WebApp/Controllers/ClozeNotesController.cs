using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClozeNotesController(IClozeNoteRepository clozeNoteRepository) : ControllerBase
{
    private readonly IClozeNoteRepository _clozeNoteRepository = clozeNoteRepository;

    // GET: api/ClozeNotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClozeNote>>> GetClozeNotes()
    {
        return await _clozeNoteRepository.GetClozeNotesAsync();
    }

    // GET: api/ClozeNotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClozeNote>> GetClozeNote(string id)
    {
        ClozeNote? clozeNote = await _clozeNoteRepository.GetClozeNoteAsync(id);

        if (clozeNote == null)
        {
            return NotFound();
        }

        return clozeNote;
    }

    // PUT: api/ClozeNotes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<ClozeNote>> PutClozeNote(string id, ClozeNote clozeNote)
    {
        if (id != clozeNote.Id)
        {
            return BadRequest();
        }
        ClozeNote? currentClozeNote = await _clozeNoteRepository.GetClozeNoteAsync(id);

        if (currentClozeNote == null)
        {
            return NotFound();
        }

        try
        {
            return await _clozeNoteRepository.UpdateOrderedElementAsync(currentClozeNote, clozeNote);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ClozeNoteExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: api/ClozeNotes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ClozeNote>> PostClozeNote(ClozeNote clozeNote)
    {
        try
        {
            await _clozeNoteRepository.InsertOrderedElementAsync(clozeNote);
        }
        catch (DbUpdateException)
        {
            if (await ClozeNoteExists(clozeNote.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetClozeNote", new { id = clozeNote.Id }, clozeNote);
    }

    // DELETE: api/ClozeNotes/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClozeNote(string id)
    {
        ClozeNote? clozeNote = await _clozeNoteRepository.GetClozeNoteAsync(id);
        if (clozeNote == null)
        {
            return NotFound();
        }

        await _clozeNoteRepository.DeleteOrderedElementAsync(clozeNote);

        return NoContent();
    }

    private async Task<bool> ClozeNoteExists(string id)
    {
        return await _clozeNoteRepository.ClozeNoteExists(id);
    }
}
