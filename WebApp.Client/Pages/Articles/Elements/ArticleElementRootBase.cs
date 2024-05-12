using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using AnkiBooks.ApplicationCore;
using AnkiBooks.WebApp.Client.Services;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class ArticleElementRootBase<T> : ComponentBase where T : ArticleElement
{
    [Inject]
    public required IArticleElementService ArticleElementService { get; set; }

    [Inject]
    public required DraggedItemHolder<ArticleElement> DraggedItemHolder { get; set; }

    [Parameter]
    public required T ArticleElement { get; set; }

    [Parameter]
    public required OrderedElementsContainer<ArticleElement> ElementsContainer { get; set; }

    [Parameter]
    public EventCallback<OrderedElementsContainer<ArticleElement>> ElementsContainerChanged { get; set; }

    protected override void OnParametersSet()
    {
        Editing = false;
        showingOptions = false;

    }

    public bool Editing { get; set; } = false;

    protected void StartEditing()
    {
        Editing = true;
    }

    protected bool showingOptions = false;

    protected void ShowOptions()
    {
        showingOptions = true;
    }

    protected void CancelShowingOptions()
    {
        showingOptions = false;
    }

    protected async Task DeleteArticleElement()
    {
        await ArticleElementService.DeleteArticleElement(ArticleElement);

        ElementsContainer.Remove(ArticleElement);
        await ElementsContainerChanged.InvokeAsync(ElementsContainer);
    }
}