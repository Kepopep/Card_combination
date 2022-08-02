using Project.Core.Entities.Controllers;
using System.Collections.Generic;

namespace Project.Core.Deck
{
    internal abstract class InteractableDeckBase
    {
        protected List<CardController> _cards;

        protected CardGame _game;

        public InteractableDeckBase(List<CardController> cards, CardGame game)
        {
            _cards = cards;
            _game = game; 

            foreach (var item in _cards)
            {
                item.OnInteract += HandleCardClick;
            }
        }

        protected abstract void HandleCardClick(CardController cardController);

        public abstract void Init();
    }
}
