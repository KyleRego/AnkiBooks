# Architecture

Anki Books is a Blazor app that takes advantage of static server-side rendering, including prerendering, and client-side rendering where the HTML is generated on the client by the Blazor WebAssembly runtime. Enhanced navigation is currently disabled as it was causing issues with UI flickering

## Radzen Blazor Components

## CSS

## Identity

The `blazor` template was used to scaffold the app initially: `dotnet new blazor -int WebAssembly --auth Individual -o WebApp`. This created the Identity Razor components