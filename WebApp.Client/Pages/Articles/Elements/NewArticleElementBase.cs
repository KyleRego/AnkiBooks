using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Enums;
using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class NewArticleElementBase<T> : ComponentBase where T : ArticleElement, new()
{
    [Inject]
    public required IArticleElementService ArticleElementService { get; set; }

    [CascadingParameter(Name="ArticleId")]
    public required string ArticleId { get; set; }

    [Parameter]
    public required ArticleElementType? DropDownItemSelected { get; set; }

    [Parameter]
    public required EventCallback<ArticleElementType?> DropDownItemSelectedChanged { get; set; }

    [Parameter]
    public required int OrdinalPosition { get; set; }

    [Parameter]
    public required OrderedElementsContainer<ArticleElement> ElementsContainer { get; set; }

    [Parameter]
    public required EventCallback<OrderedElementsContainer<ArticleElement>?> ElementsContainerChanged { get; set; }

    protected T InitialArticleElement()
    {
        return new()
        {
            ArticleId = ArticleId
        };
    }

    protected async Task SubmitForm(T newArtElement)
    {
        newArtElement.OrdinalPosition = OrdinalPosition;

        T? createdArtElement = (T?)await ArticleElementService.PostArticleElement(newArtElement);
        ArgumentNullException.ThrowIfNull(createdArtElement);

        ElementsContainer.Add(createdArtElement);
        await ElementsContainerChanged.InvokeAsync(ElementsContainer);
        DropDownItemSelected = null;
        await DropDownItemSelectedChanged.InvokeAsync(DropDownItemSelected);
    }

    protected async Task Cancel()
    {
        DropDownItemSelected = null;
        await DropDownItemSelectedChanged.InvokeAsync(DropDownItemSelected);
    }
}