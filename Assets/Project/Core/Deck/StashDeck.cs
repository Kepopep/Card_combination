using Project.Core.Entities.Controllers;
using System.Collections.Generic;

namespace Project.Core.Deck
{
    internal class StashDeck : InteractableDeckBase
    {
        public StashDeck(List<CardController> cards, CardGame game) : base(cards, game)
        {
            _game.ChangeCurrentCard(cards[0]);
        }

        protected override void HandleCardClick(CardController cardController)
        {
            _game.ChangeCurrentCard(cardController);
            _cards.Remove(cardController); //TODO wtf is this (:
        }
    }
}
