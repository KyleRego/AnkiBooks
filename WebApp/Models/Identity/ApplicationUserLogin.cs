using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}
