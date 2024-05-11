using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements;

public class ArticleElementFormBase<T> : ComponentBase where T : ArticleElement
{
    [SupplyParameterFromForm]
    public required T ArticleElement { get; set; }

    [Parameter]
    public T InitialArticleElement { get; set; } = null!;

    protected override void OnInitialized()
    {
        ArticleElement = InitialArticleElement;
    }

    [Parameter]
    public bool EditingExisting { get; set; }

    protected virtual string SubmitButtonText()
    {
        return EditingExisting ? "Update" : "Create";
    }

    [Parameter]
    public required Func<T, Task> ParentSubmitMethod { get; set; }

    [Parameter]
    public required Func<Task> ParentCancelMethod { get; set; }

    protected async Task SubmitForm()
    {
        await ParentSubmitMethod.Invoke(ArticleElement);
    }

    protected async Task Cancel()
    {
        await ParentCancelMethod.Invoke();
    }
}