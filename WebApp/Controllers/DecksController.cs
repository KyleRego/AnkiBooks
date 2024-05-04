using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DecksController(IDeckRepository repository) : ControllerBase
{
    private readonly IDeckRepository _repository = repository;

    // GET: api/Decks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Deck>>> GetDecks()
    {
        return await _repository.GetDecksAsync();
    }

    // GET: api/Decks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Deck>> GetDeck(string id)
    {
        Deck? deck = await _repository.GetDeckAsync(id);

        if (deck == null)
        {
            return NotFound();
        }

        return deck;
    }

    // PUT: api/Decks/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<ActionResult<Deck>> PutDeck(string id, Deck deck)
    {
        if (id != deck.Id)
        {
            return BadRequest();
        }
        Deck? currentDeck = await _repository.GetDeckAsync(id);

        if (currentDeck == null)
        {
            return NotFound();
        }

        try
        {
            return await _repository.UpdateOrderedElementAsync(currentDeck, deck);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await DeckExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }
    }

    // POST: api/Decks
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Deck>> PostDeck(Deck deck)
    {
        try
        {
            await _repository.InsertOrderedElementAsync(deck);
        }
        catch (DbUpdateException)
        {
            if (await DeckExists(deck.Id))
            {
                return Conflict();
            }
            else
            {
                throw;
            }
        }

        return CreatedAtAction("GetDeck", new { id = deck.Id }, deck);
    }

    // DELETE: api/Decks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDeck(string id)
    {
        Deck? deck = await _repository.GetDeckAsync(id);
        if (deck == null)
        {
            return NotFound();
        }

        await _repository.DeleteOrderedElementAsync(deck);

        return NoContent();
    }

    private async Task<bool> DeckExists(string id)
    {
        return await _repository.DeckExists(id);
    }
}