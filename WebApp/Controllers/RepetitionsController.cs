using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RepetitionsController(IRepetitionRepository repository) : ControllerBase
{
    private readonly IRepetitionRepository _repository = repository;

    [HttpPost]
    public async Task<ActionResult<Repetition>> PostRepetition(Repetition rep)
    {
        // Need to work out authorization
        // Check card id of repetition is for a card the user can access

        return await _repository.InsertRepetition(rep);
    }
}