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

        public StashDeck(List<CardController> cards, CardGame game, List<Vector2> positions) : base(cards, game) //TODO think about separetion
        {
            _positions = positions;
            _cardsQueue = new Queue<CardController>(cards);
        }

        public override void Init()
        {
            _cards[0].Open();
            SetAsHandCard(_cards[0]);
        }

        protected override void HandleCardClick(CardController cardController)
        {
            SetAsHandCard(cardController);
        }

        private void SetAsHandCard(CardController cardController)
        {
            _game.ChangeCurrentCard(cardController);
            cardController.OnInteract -= HandleCardClick;

            var firstStashCard = _cardsQueue.Dequeue();
            firstStashCard.Deactivate();

            if (_cardsQueue.Count == 0)
            {
                return;
            }

            UpdateStashPosition();

            _cardsQueue.Peek().Open();
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
