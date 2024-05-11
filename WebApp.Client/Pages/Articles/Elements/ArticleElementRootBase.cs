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

    protected async Task DeleteArticleElement()
    {
        await ArticleElementService.DeleteArticleElement(ArticleElement);

        ElementsContainer.Remove(ArticleElement);
        await ElementsContainerChanged.InvokeAsync(ElementsContainer);
    }

    protected void HandleDrag()
    {
        DraggedItemHolder.DraggedItem = ArticleElement;
    }

    protected void HandleDragEnd()
    {
        DraggedItemHolder.DraggedItem = null;
    }

    protected int nestedDragLevels = 0;

    protected void HandleDragEnter()
    {
        ArticleElement? draggedItem = DraggedItemHolder.DraggedItem;

        if (draggedItem == null) return;
        if (draggedItem == ArticleElement) return;
        nestedDragLevels += 1;
    }

    protected void HandleDragLeave()
    {
        ArticleElement? draggedItem = DraggedItemHolder.DraggedItem;

        if (draggedItem == null) return;
        if (draggedItem == ArticleElement) return;
        nestedDragLevels -= 1;
    }

    protected async Task HandleDrop()
    {
        nestedDragLevels = 0;
        ArticleElement? droppedItem = DraggedItemHolder.DraggedItem;

        if (droppedItem == null) return;
        if (droppedItem == ArticleElement) return;

        droppedItem.OrdinalPosition = ArticleElement.OrdinalPosition;

        ArticleElement? updatedItem = await ArticleElementService.PutArticleElement(droppedItem);
        ArgumentNullException.ThrowIfNull(updatedItem);

        ElementsContainer.UpdatePosition(droppedItem);
        await ElementsContainerChanged.InvokeAsync(ElementsContainer);
    }
}