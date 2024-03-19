# Architecture

`Client` is a Blazor WebAssembly app and `Backend` is an ASP.NET Core web API.

`Models` is a class library used by both `Client` and `Backend`. Classes included into the `DbSet<T>` properties of the derived `DbContext` are EF Core entities.