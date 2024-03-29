using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}