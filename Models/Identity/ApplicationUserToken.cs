using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}