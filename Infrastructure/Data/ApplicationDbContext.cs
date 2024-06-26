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
    public DbSet<Book> Books { get; set; }

    public DbSet<Article> Articles { get; set; }
    
    public DbSet<Card> Cards { get; set; }
    public DbSet<BasicNote> BasicNotes { get; set; }
    public DbSet<ClozeNote> ClozeNotes { get; set; }

    public DbSet<ArticleElement> ArticleElements { get; set; }
    public DbSet<Deck> Decks { get; set; }
    public DbSet<MarkdownContent> MarkdownContents { get; set; }

    public DbSet<Repetition> Repetitions { get; set; }

    public DbSet<InfoSource> InfoSources { get; set; }

    public DbSet<Link> Links { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<ArticleElement>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
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