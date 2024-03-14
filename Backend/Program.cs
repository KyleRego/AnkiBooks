using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// TODO: Look into token-based authentication instead
// That rules out CSRF attacks more than Synchronizer Token Pattern
builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
                .AddIdentityCookies();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
});

builder.Services.AddAuthorizationBuilder();

builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseInMemoryDatabase("AppDb")
);

builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddApiEndpoints();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "wasm_client",
        policy => policy.WithOrigins(
            [
                builder.Configuration["BackendUrl"] ?? "http://localhost:5229",
                builder.Configuration["FrontendUrl"] ?? "http://localhost:5023/"
            ]
        ).AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    );
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // TODO: Seed database here if not already set up?
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<IdentityUser>();

app.UseCors("wasm_client");

app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/logout", async (
                        SignInManager<IdentityUser> signInManager,
                        [Microsoft.AspNetCore.Mvc.FromBody] object empty
                    ) =>
{
    if (empty != null)
    {
        await signInManager.SignOutAsync();
        return Results.Ok();
    }

    return Results.Unauthorized();
}).WithOpenApi().RequireAuthorization();

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi()
.RequireAuthorization();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
