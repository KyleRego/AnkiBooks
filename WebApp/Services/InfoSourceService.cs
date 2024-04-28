using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace AnkiBooks.WebApp.Services;

public class InfoSourceService( IInfoSourceRepository repository,
                                IUserIdProvider userIdProvider) : IInfoSourceService
{
    private readonly IInfoSourceRepository _repository = repository;
    private readonly IUserIdProvider _userIdProvider = userIdProvider;

    public async Task<List<InfoSource>?> GetInfoSources()
    {
        string? currentUserId = await _userIdProvider.GetCurrentUserId();
        if (currentUserId == null) return [];

        return await _repository.GetInfoSourcesAsync(currentUserId);
    }

    public Task<LinkSource?> PostLinkSource(LinkSource linkSource)
    {
        throw new NotImplementedException();
    }
}