using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = null!;
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = null!;
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = null!;
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

    public ICollection<Book> Books { get; } = [];

    public ICollection<Concept> Concepts { get; } = [];
}