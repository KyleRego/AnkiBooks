using System.Security.Claims;

using Microsoft.AspNetCore.Authorization;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.WebApp.Policies.Requirements;

namespace AnkiBooks.WebApp.Policies.Handlers;

public class UserOwnsArticleHandler : AuthorizationHandler<UserOwnsArticleRequirement, Article>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                    UserOwnsArticleRequirement requirement,
                                                    Article resource)
    {
        if (context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value == resource.UserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}