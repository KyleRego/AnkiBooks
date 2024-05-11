using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class EditArticleElementBase<T> : ComponentBase where T : ArticleElement
{
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
}