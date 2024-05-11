using AnkiBooks.ApplicationCore.Entities;
using Microsoft.AspNetCore.Components;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements.Decks.Cards;

public class CardFormBase<T> : ComponentBase where T : Card
{
    [Parameter]
    public required T StartingCard { get; set; }

    [SupplyParameterFromForm]
    public required T Card { get; set; }

    protected override void OnInitialized()
    {
        Card = StartingCard;
    }

    [Parameter]
    public required Func<T, Task> ParentSubmitMethod { get; set; }

    [Parameter]
    public required Func<Task> ParentCancelMethod { get; set; }

    protected async Task SubmitForm()
    {
        await ParentSubmitMethod.Invoke(Card);
    }

    protected async Task Cancel()
    {
        await ParentCancelMethod.Invoke();
    }

    [Parameter]
    public required bool EditingExisting { get; set; }
}