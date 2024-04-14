using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.ApplicationCore;
// See https://learn.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-8.0
using AnkiBooks.ApplicationCore.Identity;
using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : IdentityDbContext<
        ApplicationUser, ApplicationRole, string,
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin,
        ApplicationRoleClaim, ApplicationUserToken>(options)
{
    public DbSet<Article> Articles { get; set; } = null!;
    public DbSet<ArticleContentBase> ArticleContents { get; set; } = null!;
    public DbSet<ArticleNoteBase> ArticleNotes { get; set; } = null!;
    public DbSet<BasicNote> BasicNotes { get; set; } = null!;
    public DbSet<ClozeNote> ClozeNotes { get; set; } = null!;
    public DbSet<MarkdownContent> MarkdownContents { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<ArticleContentBase>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_ArticleElementOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
            ));

        builder
            .Entity<ArticleNoteBase>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_ArticleElementOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
            ));

        builder.Entity<ApplicationUser>(b =>
        {
            // Each User can have many UserClaims
            b.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

            // Each User can have many UserLogins
            b.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();

            // Each User can have many UserTokens
            b.HasMany(e => e.Tokens)
                .WithOne(e => e.User)
                .HasForeignKey(ut => ut.UserId)
                .IsRequired();

            // Each User can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });

        builder.Entity<ApplicationRole>(b =>
        {
            // Each Role can have many entries in the UserRole join table
            b.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Each Role can have many associated RoleClaims
            b.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();
        });
    }
}