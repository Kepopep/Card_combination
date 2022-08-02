using Project.Core.Entities.Controllers;
using System;
using System.Collections.Generic;

namespace Project.Core.Deck
{
    internal class FieldDeck : InteractableDeckBase
    {
        public FieldDeck(List<CardController> cards, CardGame game) : base(cards, game) { }

        protected override void HandleCardClick(CardController cardController)
        {
            if (cardController.CheackInteractCondition(_game.CurrentCard.CardValue.Value))
            {
                _game.ChangeCurrentCard(cardController);

                _cards.Remove(cardController);

                cardController.Deactivate();
            }
        }
    }
}
