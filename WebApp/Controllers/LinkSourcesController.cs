using System.Security.Claims;
using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LinkSourcesController(IInfoSourceRepository repository) : ControllerBase
{
    private readonly IInfoSourceRepository _repository = repository;

    private string? CurrentUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    [HttpPost]
    public async Task<ActionResult<LinkSource>> PostLinkSource(LinkSource linkSource)
    {
        string? currentUserId = CurrentUserId();
        if (currentUserId == null) Forbid();

        ArgumentNullException.ThrowIfNull(currentUserId);
        linkSource.UserId = currentUserId;

        return await _repository.InsertLinkSourceAsync(linkSource);
    }
}