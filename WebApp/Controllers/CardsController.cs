using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;
using AnkiBooks.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
public class CardsController(ICardRepository repository) : ApplicationController
{
    private readonly ICardRepository _repository = repository;

    [HttpGet("api/DueCards")]
    public async Task<ActionResult<List<Card>>> DueCards()
    {
        string? userId = CurrentUserId();
        if (userId == null) return BadRequest();

        return await _repository.GetDueCards(userId);
    }
}