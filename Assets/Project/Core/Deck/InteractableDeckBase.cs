using Project.Core.Entities.Controllers;
using System;
using System.Collections.Generic;

namespace Project.Core.Deck
{
    internal abstract class InteractableDeckBase
    {
        public event Action OnCardsCountUpdated;

        public int CardsCount => _cards.Count;

        protected List<CardController> _cards;
        protected CardGame _game;

        public InteractableDeckBase(CardGame game)
        {
            _cards = new List<CardController>();
            _game = game;
        }

        public virtual void Init(List<CardController> cards)
        {
            _cards.Clear();
            _cards.AddRange(cards);

            foreach (var item in _cards)
            {
                item.OnInteract += HandleCardClick;
            }
        }

        protected void RemoveCardFromDeck(CardController cardController)
        {
            _cards.Remove(cardController);

            OnCardsCountUpdated?.Invoke();

            cardController.OnInteract -= HandleCardClick;
        }

        protected abstract void HandleCardClick(CardController cardController);

        public abstract bool CheckInteractPossibility();
    }
}
