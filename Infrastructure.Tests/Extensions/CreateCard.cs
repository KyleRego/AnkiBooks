using AnkiBooks.ApplicationCore.Entities;

namespace AnkiBooks.Infrastructure.Tests.Extensions;

public static class CreateCardExtension
{
    public static async Task<Card> CreateCard(this ApplicationDbContext dbContext)
    {
        Deck deck = await dbContext.CreateDeck();
        Card card = new ClozeNote()
        {
            Text = "Hello {{c1::world}}",
            Deck = deck
        };
        dbContext.Cards.Add(card);
        await dbContext.SaveChangesAsync();
        return card;
    }
}