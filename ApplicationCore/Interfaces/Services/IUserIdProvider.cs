namespace AnkiBooks.ApplicationCore.Interfaces.Services;

public interface IUserIdProvider
{
    public Task<string?> GetCurrentUserId();
}