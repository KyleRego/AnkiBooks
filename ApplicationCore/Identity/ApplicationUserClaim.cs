using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}