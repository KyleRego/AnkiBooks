using Microsoft.AspNetCore.Identity;

namespace AnkiBooks.Models.Identity;

public class ApplicationRole : IdentityRole
{
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = null!;
}