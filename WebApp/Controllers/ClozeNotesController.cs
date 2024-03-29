using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore;
using AnkiBooks.Infrastructure.Data;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ClozeNotesController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/ClozeNotes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClozeNote>>> GetClozeNotes()
    {
        return await _context.ClozeNotes.ToListAsync();
    }

    // GET: api/ClozeNotes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ClozeNote>> GetClozeNote(string id)
    {
        var clozeNote = await _context.ClozeNotes.FindAsync(id);

        if (clozeNote == null)
        {
            return NotFound();
        }

        return clozeNote;
    }

    // PUT: api/ClozeNotes/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutClozeNote(string id, ClozeNote clozeNote)
    {
        if (id != clozeNote.Id)
        {
            return BadRequest();
        }

        _context.Entry(clozeNote).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ClozeNoteExists(id))
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

    // POST: api/ClozeNotes
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ClozeNote>> PostClozeNote(ClozeNote clozeNote)
    {
        try
        {
            // TODO: Must be a better way to do this, also code is duplicated in basic notes version of this action 
            Article article = await _context.Articles
                .Include(a => a.BasicNotes)
                .Include(a => a.ClozeNotes)
                .FirstAsync(a => a.Id == clozeNote.ArticleId);

            List<BasicNote> basicNotesToShift = article.BasicNotes.Where(bn => bn.OrdinalPosition >= clozeNote.OrdinalPosition).ToList();
            List<ClozeNote> clozeNotesToShift = article.ClozeNotes.Where(cn => cn.OrdinalPosition >= clozeNote.OrdinalPosition).ToList();
            foreach (BasicNote bnToShift in basicNotesToShift)
            {
                bnToShift.OrdinalPosition += 1;
            }
            foreach (ClozeNote cnToShift in clozeNotesToShift)
            {
                cnToShift.OrdinalPosition += 1;
            }
            article.ClozeNotes.Add(clozeNote);

            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            if (ClozeNoteExists(clozeNote.Id))
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
        var clozeNote = await _context.ClozeNotes.FindAsync(id);
        if (clozeNote == null)
        {
            return NotFound();
        }

        _context.ClozeNotes.Remove(clozeNote);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ClozeNoteExists(string id)
    {
        return _context.ClozeNotes.Any(e => e.Id == id);
    }
}
