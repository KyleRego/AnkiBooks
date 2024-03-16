using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; } = null!;
}