using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClozeNotesController(IClozeNoteRepository clozeNoteRepository) : ControllerBase
{
    private readonly IClozeNoteRepository _clozeNoteRepository = clozeNoteRepository;

    // PUT: api/ClozeNotes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<ClozeNote>> PutClozeNote(string id, ClozeNote clozeNote)
    {
        if (id != clozeNote.Id)
        {
            return BadRequest();
        }

        try
        {
            return await _clozeNoteRepository.UpdateClozeNoteAsync(clozeNote);
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
            return await _clozeNoteRepository.InsertClozeNoteAsync(clozeNote);
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

        await _clozeNoteRepository.DeleteClozeNoteAsync(clozeNote);

        return NoContent();
    }

    private async Task<bool> ClozeNoteExists(string id)
    {
        return await _clozeNoteRepository.ClozeNoteExists(id);
    }
}
