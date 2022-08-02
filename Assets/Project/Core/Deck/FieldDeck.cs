using Project.Core.Entities.Controllers;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Project.Core.Deck
{
    internal class FieldDeck : InteractableDeckBase
    {
        public FieldDeck(List<CardController> cards, CardGame game) : base(cards, game) { }

        public override void Init()
        {
            var cardPattern = _game.FieldGenerationInfo;
            var cardCount = cardPattern.CardCount;

            int previousIndex;
            int nextIndex;

            CardController previousCard;
            CardController nextCard;

            Vector2 position;

            for (int i = 0; i < cardCount; i++)
            {
                previousIndex = cardPattern.CardViewLinkMediators[i].PreviousIndex;
                nextIndex = cardPattern.CardViewLinkMediators[i].NextIndex;

                previousCard = previousIndex == -1 ? null : _cards[previousIndex];
                nextCard = nextIndex == -1 ? null : _cards[nextIndex];

                _cards[i].InitRelationship(previousCard, nextCard);

                position.X = cardPattern.CardViewLinkMediators[i].Position.X;
                position.Y = cardPattern.CardViewLinkMediators[i].Position.Y;
                _cards[i].SetPosition(position);
            }

            var firstCards = _cards.FindAll(x => x.Previous == null);

            foreach (var item in firstCards)
            {
                item.Open();
            }
        }

        protected override void HandleCardClick(CardController cardController)
        {
            if (cardController.CheackInteractCondition(_game.CurrentCard.CardValue.Value))
            {
                if(cardController.Next != null)
                {
                    cardController.Next.Open();
                }

                _game.ChangeCurrentCard(cardController);

                _cards.Remove(cardController);

                cardController.Deactivate();
            }
        }
    }
}
