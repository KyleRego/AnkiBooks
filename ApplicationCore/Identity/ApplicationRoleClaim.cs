using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; } = null!;
}