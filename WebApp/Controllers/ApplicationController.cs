using System.Security.Claims;

using Microsoft.AspNetCore.Mvc;

namespace AnkiBooks.WebApp.Controllers;

public abstract class ApplicationController : ControllerBase
{
    protected string? CurrentUserId()
    {
        return HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}