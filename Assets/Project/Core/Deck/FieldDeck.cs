using Project.Core.Entities.Controllers;
using System.Collections.Generic;
using System.Numerics;

namespace Project.Core.Deck
{
    internal class FieldDeck : InteractableDeckBase
    {
        public FieldDeck(CardGame game) : base(game) { }

        public override bool CheckInteractPossibility()
        {
            var openedCards = _cards.FindAll(x => x.Model.State == Interfaces.ICardModel.State.Open);

            var result = false;

            foreach (var item in openedCards)
            {
                result |= item.Model.CardValue.CanUseLikeCombination(_game.CurrentCard.CardValue.Value);
            }

            return result;
        }

        public override void Init(List<CardController> cards)
        {
            base.Init(cards);

            var cardPattern = _game.FieldPattern;
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
                item.ActivateInteractions();
                item.Open();
            }
        }

        protected override void HandleCardClick(CardController cardController)
        {
            if (cardController.CheackInteractCondition(_game.CurrentCard.CardValue.Value))
            {
                if(cardController.Next != null)
                {
                    cardController.Next.ActivateInteractions();
                    cardController.Next.Open();
                }

                _game.ChangeCurrentCard(cardController);

                RemoveCardFromDeck(cardController);

                cardController.DeactivateInteractions();
            }
        }
    }
}
