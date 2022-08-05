using Project.Core.Entities.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Project.Core.Deck
{
    internal class StashDeck : InteractableDeckBase
    {
        private Queue<CardController> _cardsQueue;
        private List<Vector2> _positions;

        public StashDeck(CardGame game, List<Vector2> positions) : base(game)
        {
            _positions = positions;
        }

        public override bool CheckInteractPossibility()
        {
            return _cards.Count != 0;
        }

        public override void Init(List<CardController> cards)
        {
            base.Init(cards);

            _cardsQueue = new Queue<CardController>(_cards);

            TakeTopCard();
        }

        protected override void HandleCardClick(CardController cardController)
        {
            TakeTopCard();
        }

        private void TakeTopCard()
        {
            var topCard = _cardsQueue.Dequeue();

            topCard.Open();
            topCard.DeactivateInteractions();

            _game.ChangeCurrentCard(topCard);

            if (_cardsQueue.Count != 0)
            {
                UpdateStashPosition();

                _cardsQueue.Peek().ActivateInteractions();
            }

            RemoveCardFromDeck(topCard);
        }

        private void UpdateStashPosition()
        {
            var index = 0;
            var stashCardPositions = new List<Vector2>();

            foreach (var item in _cardsQueue)
            {
                stashCardPositions.Add(_positions[index]);
                index = index + 1 < _positions.Count - 1 ? index + 1 : _positions.Count - 1;
            }

            index = _cardsQueue.Count - 1;

            foreach (var item in _cardsQueue.Reverse())
            {
                item.SetPosition(stashCardPositions[index]);
                index--;
            }
        }
    }
}
