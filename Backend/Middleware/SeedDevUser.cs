using AnkiBooks.Backend.Database;
using AnkiBooks.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Backend.Middleware;

public class SeedDevUser
{
    private readonly RequestDelegate _next;

    public SeedDevUser(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, ApplicationDbContext dbContext)
    {
        ApplicationUser? testUser = dbContext.Users.FirstOrDefault(u => u.Email == "test@example.com");

        if (testUser == null)
        {
            PasswordHasher<ApplicationUser> passwordHasher = new();

            testUser = new()
            {
                Email = "test@example.com"
            };

            string hash = passwordHasher.HashPassword(testUser, "Asdf333!");

            testUser.PasswordHash = hash;

            dbContext.Users.Add(testUser);
            dbContext.SaveChanges();
        }

        // Call the next delegate/middleware in the pipeline.
        await _next(httpContext);
    }
}

public static class SeedDevUserMiddlewareExtensions
{
    public static IApplicationBuilder UseSeedDevUserMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SeedDevUser>();
    }
}