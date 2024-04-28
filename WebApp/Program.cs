using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AnkiBooks.WebApp.Components;
using AnkiBooks.WebApp.Components.Account;
using AnkiBooks.Infrastructure.Data;
using AnkiBooks.ApplicationCore.Identity;
using AnkiBooks.ApplicationCore.Interfaces;
using AnkiBooks.Infrastructure.Repository;
using AnkiBooks.WebApp.Client;
using AnkiBooks.WebApp.Services;
using AnkiBooks.ApplicationCore.Interfaces.Services;
using System.Security.Principal;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

CommonServices.Configure(builder.Services, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRazorComponents().AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
    options.UseSqlite(builder.Configuration.GetConnectionString("Database"));
    options.EnableSensitiveDataLogging();
    }
);
builder.Services.AddScoped<IUserArticleRepository, UserArticleRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBasicNoteRepository, BasicNoteRepository>();
builder.Services.AddScoped<IClozeNoteRepository, ClozeNoteRepository>();
builder.Services.AddScoped<IMarkdownContentRepository, MarkdownContentRepository>();
builder.Services.AddScoped<ILinkRepository, LinkRepository>();

builder.Services.AddScoped<IUserArticleService, ServerUserArticleService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<ILinkService, LinkService>();
builder.Services.AddScoped<IUserIdProvider, UserIdProvider>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    ApplicationDbContext context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();

    string testUserEmail = "test@example.com";
    string testUserPassword = "Asdf333!";
    ApplicationUser? testUser = context.Users.FirstOrDefault(u => u.Email == testUserEmail);

    if (testUser == null)
    {
        PasswordHasher<ApplicationUser> passwordHasher = new();

        testUser = new()
        {
            Email = testUserEmail,
            NormalizedEmail = testUserEmail.ToUpper(),
            UserName = testUserEmail,
            NormalizedUserName = testUserEmail.ToUpper()
        };

        string hash = passwordHasher.HashPassword(testUser, testUserPassword);

        testUser.PasswordHash = hash;

        context.Users.Add(testUser);
        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(AnkiBooks.WebApp.Client._Imports).Assembly);
app.MapAdditionalIdentityEndpoints();

app.Run();

public partial class Program { }