# Architecture

Anki Books is a Blazor Web App that uses mainly client-side rendering but also static server-side rendering and prerendering. Enhanced navigation is currently disabled as it was causing issues with UI flickering. The project organization is following a clean architecture model but this will probably change to something simpler.

To run the app, have the .NET 8 runtime installed and `dotnet run` from the WebApp project root will start the app