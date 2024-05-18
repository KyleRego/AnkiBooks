using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.ApplicationCore.Repository;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Services;

public class CardService(ICardRepository repository, IUserIdProvider userIdProvider) : ICardService
{
    private readonly ICardRepository _repository = repository;

    private readonly IUserIdProvider _userIdProvider = userIdProvider;

    public Task DeleteCard(Card card)
    {
        throw new NotImplementedException();
    }

    public Task<Card?> PostCard(Card card)
    {
        throw new NotImplementedException();
    }

    public Task<Card?> PutCard(Card card)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Card>?> GetDueCards()
    {
        string? userId = await _userIdProvider.GetCurrentUserId();
        ArgumentNullException.ThrowIfNull(userId);

        return await _repository.GetDueCards(userId);
    }
}