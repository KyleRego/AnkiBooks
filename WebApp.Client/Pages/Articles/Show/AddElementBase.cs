using AnkiBooks.ApplicationCore;
using AnkiBooks.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Components;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Show;

/// <summary>
/// Base class for the Razor components related
/// to adding new elements to the article that
/// have some shared properties that are injected
/// as parameters through the component hierarchy
/// - Show
///     - AddArticleElementSplitButton
///         - NewBasicNote
///         - NewClozeNote
/// </summary>
public abstract class AddElementBase : ComponentBase
{
    [Parameter]
    public string ArticleId { get; set; } = null!;

    /// <summary>
    /// The ordinal position of PairedElement is interrogated from the
    /// OrderedElementsContainer to determine the OrdinalPosition to POST.
    /// 
    /// This is null for the last button which does not have a paired element.
    /// </summary>
    [Parameter]
    public IArticleElement? PairedElement { get; set; }

    [Parameter]
    public OrderedElementsContainer OrderedElementsContainer { get; set; } = null!;

    [Parameter]
    public EventCallback<OrderedElementsContainer> OrderedElementsContainerChanged { get; set; }
}