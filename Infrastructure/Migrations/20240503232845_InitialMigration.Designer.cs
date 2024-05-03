﻿// <auto-generated />
using System;
using AnkiBooks.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240503232845_InitialMigration")]
    partial class InitialMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Article", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentArticleId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Public")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentArticleId");

                    b.HasIndex("UserId");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.ArticleElement", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ArticleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(21)
                        .HasColumnType("TEXT");

                    b.Property<int>("OrdinalPosition")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("ArticleElements", t =>
                        {
                            t.HasCheckConstraint("CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0");
                        });

                    b.HasDiscriminator<string>("Discriminator").HasValue("ArticleElement");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Book", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PublicViewable")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Link", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Complete")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Url")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationRoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserLogin", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserToken", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.BasicNote", b =>
                {
                    b.HasBaseType("AnkiBooks.ApplicationCore.Entities.ArticleElement");

                    b.Property<string>("Back")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Front")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasIndex("ArticleId");

                    b.ToTable(t =>
                        {
                            t.HasCheckConstraint("CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0");
                        });

                    b.HasDiscriminator().HasValue("BasicNote");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.ClozeNote", b =>
                {
                    b.HasBaseType("AnkiBooks.ApplicationCore.Entities.ArticleElement");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasIndex("ArticleId");

                    b.ToTable(t =>
                        {
                            t.HasCheckConstraint("CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0");
                        });

                    b.HasDiscriminator().HasValue("ClozeNote");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.MarkdownContent", b =>
                {
                    b.HasBaseType("AnkiBooks.ApplicationCore.Entities.ArticleElement");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasIndex("ArticleId");

                    b.ToTable("ArticleElements", t =>
                        {
                            t.HasCheckConstraint("CK_SectionOrdinalPositionIsNotNegative", "[OrdinalPosition] >= 0");

                            t.Property("Text")
                                .HasColumnName("MarkdownContent_Text");
                        });

                    b.HasDiscriminator().HasValue("MarkdownContent");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Article", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Entities.Article", "ParentArticle")
                        .WithMany("ChildArticles")
                        .HasForeignKey("ParentArticleId");

                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("Articles")
                        .HasForeignKey("UserId");

                    b.Navigation("ParentArticle");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Book", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", null)
                        .WithMany("Books")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Link", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("Links")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationRoleClaim", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationRole", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserClaim", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserLogin", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserRole", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUserToken", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Identity.ApplicationUser", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.BasicNote", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Entities.Article", "Article")
                        .WithMany("BasicNotes")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.ClozeNote", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Entities.Article", "Article")
                        .WithMany("ClozeNotes")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.MarkdownContent", b =>
                {
                    b.HasOne("AnkiBooks.ApplicationCore.Entities.Article", "Article")
                        .WithMany("MarkdownContents")
                        .HasForeignKey("ArticleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Article");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Entities.Article", b =>
                {
                    b.Navigation("BasicNotes");

                    b.Navigation("ChildArticles");

                    b.Navigation("ClozeNotes");

                    b.Navigation("MarkdownContents");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationRole", b =>
                {
                    b.Navigation("RoleClaims");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("AnkiBooks.ApplicationCore.Identity.ApplicationUser", b =>
                {
                    b.Navigation("Articles");

                    b.Navigation("Books");

                    b.Navigation("Claims");

                    b.Navigation("Links");

                    b.Navigation("Logins");

                    b.Navigation("Tokens");

                    b.Navigation("UserRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
