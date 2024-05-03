using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace AnkiBooks.WebApp.Services;

public class LinkService( ILinkRepository repository,
                                IUserIdProvider userIdProvider) : ILinkService
{
    private readonly ILinkRepository _repository = repository;
    private readonly IUserIdProvider _userIdProvider = userIdProvider;

    public async Task<List<Link>?> GetLinks()
    {
        string? currentUserId = await _userIdProvider.GetCurrentUserId();
        if (currentUserId == null) return [];

        return await _repository.GetLinksAsync(currentUserId);
    }

    public Task<Link?> PostLink(Link link)
    {
        throw new NotImplementedException();
    }
}