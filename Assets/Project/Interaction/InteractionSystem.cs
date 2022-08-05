using Project.Core.Entities.Controllers;
using Project.Data;
using System;
using System.Collections.Generic;
using System.Numerics;

using static Project.Interfaces.ICardModel;

namespace Project.Interaction
{
    internal class InteractionSystem
    {
        private struct CardCollision
        {
            public CardController Controller;
            public Vector2[] EdgePositions;

            public CardCollision(CardController controller, Vector2 size)
            {
                Controller = controller;

                EdgePositions = new Vector2[2];
                EdgePositions[0] = Controller.Model.Position - size / 2f;
                EdgePositions[1] = Controller.Model.Position + size / 2f;
            }

            public bool IsPointInside(Vector2 point)
            {
                return EdgePositions[0].X <= point.X &&
                        EdgePositions[1].X >= point.X &&
                        EdgePositions[0].Y <= point.Y &&
                        EdgePositions[1].Y >= point.Y;
            }
        }

        public event Action<CardController> OnClick;

        private DoubleBuffer<CardCollision> _cardsBuffer;

        private Vector2 _colliderSize = new Vector2(1.45f, 2.1f);

        public InteractionSystem()
        {
            _cardsBuffer = new DoubleBuffer<CardCollision>();
        }

        public void InitInteraction(List<CardController> cards)
        {
            foreach (var item in cards)
            {
                item.OnActivated += ActivateCardInteracliton;
                item.OnDeactivated += DeactivateCardInteraction;
            }

            var openedCard = cards.FindAll(x => x.Model.State == State.Open || x.Model.State == State.Active);

            foreach (var item in openedCard)
            {
                ActivateCardInteracliton(item);
            }
        }

        public void Click(Vector2 clickPosition)
        {
            var clickedCard = _cardsBuffer.Current.Find(x => x.IsPointInside(clickPosition));

            if (clickedCard.Controller != null)
            {
                OnClick?.Invoke(clickedCard.Controller);
            }
        }

        public void Update()
        {
            _cardsBuffer.Swap();
        }

        public void Reset()
        {
            _cardsBuffer.Clear();
        }
            
        private void ActivateCardInteracliton(CardController cardController)
        {
            var collision = new CardCollision(cardController, _colliderSize);

            _cardsBuffer.Add(collision);
        }

        private void DeactivateCardInteraction(CardController cardController)
        {
            var collisionToRemove = _cardsBuffer.Current.Find(x => x.Controller == cardController);

            _cardsBuffer.Remove(collisionToRemove);

            cardController.OnDeactivated -= DeactivateCardInteraction;
            cardController.OnOpen -= ActivateCardInteracliton;
        }
    }
}
