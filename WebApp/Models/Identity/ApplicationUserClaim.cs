using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationUserClaim : IdentityUserClaim<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}