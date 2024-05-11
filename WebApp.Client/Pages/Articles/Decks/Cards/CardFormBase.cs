using AnkiBooks.ApplicationCore.Entities;
using Microsoft.AspNetCore.Components;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Decks.Cards;

public class CardFormBase<T> : ComponentBase where T : Card
{
    [Parameter]
    public T? StartingCard { get; set; }

    [SupplyParameterFromForm]
    public T? Card { get; set; }

    protected override void OnInitialized()
    {
        Card = StartingCard;
    }

    [Parameter]
    public Func<T, Task> ParentSubmitMethod { get; set; } = null!;

    [Parameter]
    public Func<Task> ParentCancelMethod { get; set; } = null!;

    protected async Task SubmitForm()
    {
        ArgumentNullException.ThrowIfNull(Card);
        await ParentSubmitMethod.Invoke(Card);
    }

    protected async Task Cancel()
    {
        await ParentCancelMethod.Invoke();
    }

    [Parameter]
    public bool EditingExisting { get; set; }
}