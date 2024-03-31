dotnet test --collect:"XPlat Code Coverage"

reportgenerator -reports:"WebApp.Tests/**/coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html