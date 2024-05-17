# Ankibooks.Infrastructure

## Designer files

The .Designer.cs files in Migrations are necessary for `db.Database.Migrate()` in the `WebApp` project `Program.cs`.

## dotnet-ef

To scaffold migrations, the dotnet-ef tool must be installed

To install it globally:
```
dotnet tool install --global dotnet-ef
```

To update this to the latest version:
```
dotnet tool update --global dotnet-ef
```

Verify it is correctly installed with `dotnet ef`, it may be necessary to close and reopen your IDE or terminal.