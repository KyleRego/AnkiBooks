using AnkiBooks.ApplicationCore.Entities;
using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationUser : IdentityUser
{
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = null!;
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = null!;
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = null!;
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

    public List<Article> Articles { get; set; } = [];
    public List<Book> Books { get; set; } = [];
    public List<LinkSource> LinkSources { get; set; } = [];
}