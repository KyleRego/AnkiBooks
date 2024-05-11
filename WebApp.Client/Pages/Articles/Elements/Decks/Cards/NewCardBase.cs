using Microsoft.AspNetCore.Components;

using AnkiBooks.ApplicationCore.Entities;
using AnkiBooks.ApplicationCore.Services;
using AnkiBooks.ApplicationCore.Enums;

namespace AnkiBooks.WebApp.Client.Pages.Articles.Elements.Decks.Cards;

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
    public required CardType? DropDownItemSelected { get; set; }

    [Parameter]
    public required EventCallback<CardType?> DropDownItemSelectedChanged { get; set; }

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
        
        ClozeNote? createdClozeNote = (ClozeNote?)await CardService.PostCard(newCard);
        ArgumentNullException.ThrowIfNull(createdClozeNote);

        DropDownItemSelected = null;
        await DropDownItemSelectedChanged.InvokeAsync(DropDownItemSelected);

        Cards.Add(createdClozeNote);
        await CardsChanged.InvokeAsync(Cards);
    }

    protected async Task Cancel()
    {
        DropDownItemSelected = null;
        await DropDownItemSelectedChanged.InvokeAsync(DropDownItemSelected);
    }
}