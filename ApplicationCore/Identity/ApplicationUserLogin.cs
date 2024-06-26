using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}
