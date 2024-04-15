using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SectionsController(ISectionRepository sectionRepository) : ControllerBase
{
    private readonly ISectionRepository _sectionRepository = sectionRepository;

    // GET: api/Sections
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Section>>> GetSections()
    {
        return await _sectionRepository.GetSectionsAsync();
    }

    // GET: api/Sections/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Section>> GetSection(string id)
    {
        Section? section = await _sectionRepository.GetSectionAsync(id);

        if (section == null)
        {
            return NotFound();
        }

        return section;
    }

    // PUT: api/Sections/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<Section>> PutSection(string id, Section section)
    {
        if (id != section.Id)
        {
            return BadRequest();
        }
        Section? currentContent = await _sectionRepository.GetSectionAsync(id);

        if (currentContent == null)
        {
            return NotFound();
        }

        try
        {
            return await _sectionRepository.UpdateOrderedElementAsync(currentContent, section);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await SectionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: api/Sections
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Section>> PostSection(Section section)
    {
        try
        {
            await _sectionRepository.InsertOrderedElementAsync(section);
        }
        catch (DbUpdateException)
        {
            if (await SectionExists(section.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetSection", new { id = section.Id }, section);
    }

    // DELETE: api/Sections/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSection(string id)
    {
        Section? section = await _sectionRepository.GetSectionAsync(id);
        if (section == null)
        {
            return NotFound();
        }

        await _sectionRepository.DeleteOrderedElementAsync(section);

        return NoContent();
    }

    private async Task<bool> SectionExists(string id)
    {
        return await _sectionRepository.SectionExists(id);
    }
}
