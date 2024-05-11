using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Decks.Cards;

public class EditCardBase<T> : ComponentBase where T : Card
{
    [Inject]
    public required ICardService CardService { get; set; }

    [Parameter]
    public T? InitialCard { get; set; }

    [Parameter]
    public bool EditingCard { get; set; }

    [Parameter]
    public EventCallback<bool> EditingCardChanged { get; set; }

    protected async Task SubmitForm(T editedCard)
    {
        ArgumentNullException.ThrowIfNull(editedCard.DeckId);

        T? updatedCard = (T?)await CardService.PutCard(editedCard);
        ArgumentNullException.ThrowIfNull(updatedCard);

        EditingCard = false;
        await EditingCardChanged.InvokeAsync(EditingCard);
    }

    protected async Task Cancel()
    {
        EditingCard = false;
        await EditingCardChanged.InvokeAsync(EditingCard);
    }
}