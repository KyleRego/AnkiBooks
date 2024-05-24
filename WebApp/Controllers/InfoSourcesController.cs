using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Repository;

namespace AnkiBooks.WebApp.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class InfoSourcesController(IInfoSourceRepository repository) : ControllerBase
{
    private readonly IInfoSourceRepository _repository = repository;

    public async Task<ActionResult<List<InfoSource>>> Index(int pageNumber)
    {
        if (pageNumber < 1) pageNumber = 1;

        return await _repository.GetInfoSourcesAsync(pageNumber);
    }
}