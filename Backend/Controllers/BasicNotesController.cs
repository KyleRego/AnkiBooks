using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.Backend.DbContext;
using AnkiBooks.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicNotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BasicNotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BasicNotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BasicNote>>> GetBasicNotes()
        {
            return await _context.BasicNotes.ToListAsync();
        }

        // GET: api/BasicNotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BasicNote>> GetBasicNote(string id)
        {
            var basicNote = await _context.BasicNotes.FindAsync(id);

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

            _context.Entry(basicNote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasicNoteExists(id))
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

        // POST: api/BasicNotes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<BasicNote>> PostBasicNote(BasicNote basicNote)
        {
            _context.BasicNotes.Add(basicNote);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (BasicNoteExists(basicNote.Id))
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
            var basicNote = await _context.BasicNotes.FindAsync(id);
            if (basicNote == null)
            {
                return NotFound();
            }

            _context.BasicNotes.Remove(basicNote);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BasicNoteExists(string id)
        {
            return _context.BasicNotes.Any(e => e.Id == id);
        }
    }
}
