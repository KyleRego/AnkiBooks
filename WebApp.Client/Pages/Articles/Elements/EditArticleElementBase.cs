using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class EditArticleElementBase<T> : ComponentBase where T : ArticleElement
{
    [Inject]
    public required IArticleElementService ArticleElementService { get; set; }

    [Parameter]
    public required OrderedElementsContainer<ArticleElement> ElementsContainer { get; set; }

    [Parameter]
    public required EventCallback<OrderedElementsContainer<ArticleElement>> ElementsContainerChanged { get; set; }

    [Parameter]
    public required T InitialArticleElement { get; set; }

    [Parameter]
    public required bool Editing { get; set; }

    [Parameter]
    public required EventCallback<bool> EditingChanged { get; set; }

    protected async Task SubmitForm(T editedArtElement)
    {
        T? updatedArtElement = (T?)await ArticleElementService.PutArticleElement(editedArtElement);
        ArgumentNullException.ThrowIfNull(updatedArtElement);

        Editing = false;
        await EditingChanged.InvokeAsync(Editing);
    }

    protected async Task Cancel()
    {
        Editing = false;
        await EditingChanged.InvokeAsync(Editing);
    }
}