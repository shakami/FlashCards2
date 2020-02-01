using FlashCards.Api.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlashCards.Api.Repository
{
    public class FlashCardRepository : IFlashCardRepository
    {
        private readonly JsonDataContext _context;

        public FlashCardRepository() : this("./Data/data.json")
        { }

        public FlashCardRepository(string dataPath)
        {
            _context = new JsonDataContext(dataPath);
        }

        public Deck AddDeck(Deck newDeck)
        {
            if (newDeck is null)
            {
                throw new ArgumentNullException(nameof(newDeck));
            }

            newDeck.Id = _context.GetNewDeckId();
            foreach (var card in newDeck.Cards)
            {
                card.Id = _context.GetNewFlashCardId();
                card.DeckId = newDeck.Id;
            }
            _context.Decks.Add(newDeck);
            return newDeck;
        }

        public IEnumerable<Deck> GetDecks()
        {
            return _context.Decks;
        }

        public Deck GetDeck(int deckId)
        {
            return _context.Decks.FirstOrDefault(d => d.Id == deckId);
        }

        public Deck UpdateDeck(Deck updatedDeck)
        {
            // no code needed
            return updatedDeck;
        }

        public void DeleteDeck(Deck deck)
        {
            if (deck is null)
            {
                throw new ArgumentNullException(nameof(deck));
            }

            _context.Decks.Remove(deck);
        }

        public Card AddCard(Card newCard, int deckId)
        {
            if (newCard is null)
            {
                throw new ArgumentNullException(nameof(newCard));
            }

            var deck = GetDeck(deckId);
            if (deck == null)
            {
                return null;
            }

            newCard.DeckId = deckId;
            newCard.Id = _context.GetNewFlashCardId();
            deck.Add(newCard);
            return newCard;
        }

        public IEnumerable<Card> GetCards(int deckId)
        {
            return GetDeck(deckId)?.Cards;
        }

        public Card GetCard(int cardId)
        {
            return _context.Decks
                .SelectMany(d => d.Cards)
                .FirstOrDefault(f => f.Id == cardId);
        }

        public Card UpdateCard(Card updatedCard)
        {
            // no code needed
            return updatedCard;
        }

        public void DeleteCard(Card cardToDelete)
        {
            if (cardToDelete is null)
            {
                throw new ArgumentNullException(nameof(cardToDelete));
            }

            var deck = GetDeck(cardToDelete.DeckId);
            deck.Remove(cardToDelete);
        }

        public bool DeckExists(int deckId)
        {
            return _context.Decks.Any(d => d.Id == deckId);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
