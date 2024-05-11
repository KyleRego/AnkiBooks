using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Enums;
using AnkiBooks.ApplicationCore;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class NewArticleElementBase<T> : ComponentBase where T : ArticleElement, new()
{
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

    protected async Task AddArticleElementToArticle(ArticleElement newArtElement)
    {
        ElementsContainer.Add(newArtElement);
        await ElementsContainerChanged.InvokeAsync(ElementsContainer);
        DropDownItemSelected = null;
        await DropDownItemSelectedChanged.InvokeAsync(DropDownItemSelected);
    }
}