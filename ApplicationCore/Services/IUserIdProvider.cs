namespace AnkiBooks.ApplicationCore.Services;

public interface IUserIdProvider
{
    public Task<string?> GetCurrentUserId();
}