# Architecture

`Client` is a Blazor WebAssembly app and `Backend` is an ASP.NET Core web API. Both can be started with `dotnet run` (the .NET SDK must be installed first, see [here](https://learn.microsoft.com/en-us/dotnet/core/get-started)).

`Models` is a class library used by both `Client` and `Backend`. Classes included into the `DbSet<T>` properties of the derived `DbContext` are EF Core entities.

## Client

`Client` was scaffolded with the `blazorwasm` template ([project structure](https://learn.microsoft.com/en-us/aspnet/core/blazor/project-structure?view=aspnetcore-8.0#blazor-webassembly)) and [BlazorBootstrap](https://demos.blazorbootstrap.com/) was added.

### CSS Configuration

Bootstrap can be configured by overriding the default values for the CSS variables in `wwwroot/scss/custom.css` (`npm install -g sass` to install Sass globally to run the command):

```
sass --watch ./scss/custom.scss ./css/custom.css --style compressed
```

The `<Sidebar>` Razor component from BlazorBootstrap can be configured with the `<script>` tag in the component which wraps it.