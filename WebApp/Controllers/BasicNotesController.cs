using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BasicNotesController(IBasicNoteRepository basicNoteRepository) : ControllerBase
{
    private readonly IBasicNoteRepository _basicNoteRepository = basicNoteRepository;

    // PUT: api/BasicNotes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<BasicNote>> PutBasicNote(string id, BasicNote basicNote)
    {
        if (id != basicNote.Id)
        {
            return BadRequest();
        }

        try
        {
            return await _basicNoteRepository.UpdateBasicNoteAsync(basicNote);
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
    }

    // POST: api/BasicNotes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<BasicNote>> PostBasicNote(BasicNote basicNote)
    {
        try
        {
            return await _basicNoteRepository.InsertBasicNoteAsync(basicNote);
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

        await _basicNoteRepository.DeleteBasicNoteAsync(basicNote);

        return NoContent();
    }

    private async Task<bool> BasicNoteExists(string id)
    {
        return await _basicNoteRepository.BasicNoteExists(id);
    }
}
