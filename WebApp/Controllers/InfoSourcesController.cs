using System.Security.Claims;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfoSourcesController(IInfoSourceRepository repository) : ControllerBase
{
    private readonly IInfoSourceRepository _repository = repository;

    private string? CurrentUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InfoSource>>> Index()
    {
        string? currentUserId = CurrentUserId();
        if (currentUserId == null) Forbid();

        ArgumentNullException.ThrowIfNull(currentUserId);

        return await _repository.GetInfoSourcesAsync(currentUserId);
    }
}