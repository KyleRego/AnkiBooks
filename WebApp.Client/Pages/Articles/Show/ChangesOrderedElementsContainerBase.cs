using AnkiBooks.ApplicationCore;
using Microsoft.AspNetCore.Components;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Show;

public abstract class ChangesOrderedElementsContainerBase : ComponentBase
{
    [Parameter]
    public OrderedElementsContainer OrderedElementsContainer { get; set; } = null!;

    [Parameter]
    public EventCallback<OrderedElementsContainer> OrderedElementsContainerChanged { get; set; }
}