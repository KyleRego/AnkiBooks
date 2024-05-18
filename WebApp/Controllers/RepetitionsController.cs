using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepetitionsController(ICardRepository cardRepository, IRepetitionRepository repRepository) : ControllerBase
{
    private readonly ICardRepository _cardRepository = cardRepository;
    private readonly IRepetitionRepository _repRepository = repRepository;

    [HttpPost]
    public async Task<ActionResult<Repetition>> PostRepetition(Repetition rep)
    {
        Card? card = await _cardRepository.GetCard(rep.CardId);
        if (card == null) return BadRequest();
        // TODO: Check card id of repetition is for a card the user can access

        int successStreak = await _cardRepository.GetSuccessfulRepetitionsStreak(card);

        card.UpdateSelfAfterRepetition(rep.Grade, successStreak);
        await _cardRepository.UpdateCardAsync(card);

        return await _repRepository.InsertRepetition(rep);
    }
}