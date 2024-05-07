using System.Security.Claims;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinksController(ILinkRepository repository) : ControllerBase
{
    private readonly ILinkRepository _repository = repository;

    private string? CurrentUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Link>>> Index()
    {
        string? currentUserId = CurrentUserId();
        if (currentUserId == null) Forbid();

        ArgumentNullException.ThrowIfNull(currentUserId);

        return await _repository.GetLinksAsync(currentUserId);
    }

    [HttpPost]
    public async Task<ActionResult<Link>> Create(Link link)
    {
        string? currentUserId = CurrentUserId();
        if (currentUserId == null) Forbid();

        ArgumentNullException.ThrowIfNull(currentUserId);
        link.UserId = currentUserId;

        return await _repository.InsertLinkAsync(link);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Link>> Update(string id, Link link)
    {
        string? currentUserId = CurrentUserId();
        if (currentUserId == null) Forbid();
        ArgumentNullException.ThrowIfNull(currentUserId);

        if (link.UserId != currentUserId) Forbid();

        if (id != link.Id)
        {
            return BadRequest();
        }

        return await _repository.UpdateLinkAsync(link);
    }
}