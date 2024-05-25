using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.WebApp.Client.Shared.Cards;

public class NewCardBase<T> : ComponentBase where T : Card, new()
{
    [Inject]
    public required ICardService CardService { get; set; }

    [Parameter]
    public required string DeckId { get; set; }

    [Parameter]
    public required List<Card> Cards { get; set; }

    [Parameter]
    public required EventCallback<List<Card>> CardsChanged { get; set; }

    [Parameter]
    public required CardTypeEnum? NewCardSelection { get; set; }

    [Parameter]
    public required EventCallback<CardTypeEnum?> NewCardSelectionChanged { get; set; }

    protected T StartingCard()
    {
        return new()
        {
            DeckId = DeckId
        };
    }

    protected async Task SubmitForm(T newCard)
    {
        ArgumentNullException.ThrowIfNull(newCard.DeckId);

        T? createdCard = (T?)await CardService.PostCard(newCard);
        ArgumentNullException.ThrowIfNull(createdCard);

        NewCardSelection = null;
        await NewCardSelectionChanged.InvokeAsync(NewCardSelection);

        Cards.Insert(0, createdCard);
        await CardsChanged.InvokeAsync(Cards);
    }

    protected async Task Cancel()
    {
        NewCardSelection = null;
        await NewCardSelectionChanged.InvokeAsync(NewCardSelection);
    }
}