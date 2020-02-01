using FlashCards.Api.Entities;
using System.Collections.Generic;

namespace FlashCards.Api.Repository
{
    public interface IFlashCardRepository
    {
        Deck AddDeck(Deck newDeck);
        IEnumerable<Deck> GetDecks();
        Deck GetDeck(int deckId);
        Deck UpdateDeck(Deck updatedDeck);
        void DeleteDeck(Deck deck);

        Card AddCard(Card newCard, int deckId);
        IEnumerable<Card> GetCards(int deckId);
        Card GetCard(int cardId);
        Card UpdateCard(Card updatedCard);
        void DeleteCard(Card card);

        bool DeckExists(int deckId);
        void Save();
    }
}
