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
    public DbSet<NoteBase> Notes { get; set; } = null!;
    public DbSet<BasicNote> BasicNotes { get; set; } = null!;
    public DbSet<ClozeNote> ClozeNotes { get; set; } = null!;
    public DbSet<ContentBase> Contents { get; set; } = null!;
    public DbSet<MarkdownContent> MarkdownContents { get; set; } = null!;
    public DbSet<Section> Sections { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<Section>()
            .HasMany(e => e.BasicNotes)
            .WithOne(e => e.Section)
            .HasForeignKey(e => e.SectionId);

        builder
            .Entity<Section>()
            .HasMany(e => e.ClozeNotes)
            .WithOne(e => e.Section)
            .HasForeignKey(e => e.SectionId);

        builder
            .Entity<Section>()
            .HasMany(e => e.MarkdownContents)
            .WithOne(e => e.Section)
            .HasForeignKey(e => e.SectionId);

        builder
            .Entity<Section>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
            ));

        builder
            .Entity<NoteBase>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_NoteOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
            ));

        builder
            .Entity<ContentBase>()
            .ToTable(b => b.HasCheckConstraint(
                "CK_ContentOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0"
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