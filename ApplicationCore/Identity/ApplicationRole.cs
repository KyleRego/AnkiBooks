using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.ApplicationCore.Identity;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = null!;
}