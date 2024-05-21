using Microsoft.AspNetCore.Authorization;

namespace AnkiBooks.WebApp.Policies.Requirements;

public class UserOwnsArticleRequirement : IAuthorizationRequirement { }