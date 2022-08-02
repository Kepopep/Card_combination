using Project.Core.Entities.Controllers;
using Project.Core.Entities.Models;
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

            public CardCollision(CardController controller)
            {
                Controller = controller;
                EdgePositions = new Vector2[2];
            }

            public void UpdateEdgePoints(Vector2 size)
            {
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

        private Vector2 _colliderSize = Vector2.One;

        public InteractionSystem()
        {
            _cardsBuffer = new DoubleBuffer<CardCollision>();
        }

        public void Init(List<CardController> cards)
        {
            foreach (var item in cards)
            {
                item.OnDeactivated += DeactivateCardInteraction;
                item.OnOpen += ActivateCardInteracliton;
            }

            var openedCard = cards.FindAll(x => x.Model.State == State.Open);

            foreach (var item in openedCard)
            {
                ActivateCardInteracliton(item);
            }
        }

        private void ActivateCardInteracliton(CardController cardController)
        {
            var collision = new CardCollision(cardController);
            collision.UpdateEdgePoints(_colliderSize);

            _cardsBuffer.Add(collision);
        }

        private void DeactivateCardInteraction(CardController cardController)
        {
            var collisionToRemove = _cardsBuffer.Current.Find(x => x.Controller == cardController);

            _cardsBuffer.Remove(collisionToRemove);
        }

        public void Click(Vector2 clickPosition)
        {
            foreach (var item in _cardsBuffer.Current)
            {
                if (item.IsPointInside(clickPosition))
                {
                    OnClick?.Invoke(item.Controller);
                    return; //TODO add layers / order by z
                }
            }
        }

        public void Update()
        {
            _cardsBuffer.Swap();
        }

        public void DrawDebug()
        {
            foreach (var item in _cardsBuffer.Current)
            {
                UnityEngine.Debug.DrawLine(ToUnityVector(item.EdgePositions[0]), ToUnityVector(item.EdgePositions[0] + new Vector2(_colliderSize.X, 0)));
                UnityEngine.Debug.DrawLine(ToUnityVector(item.EdgePositions[0] + new Vector2(_colliderSize.X, 0)), ToUnityVector(item.EdgePositions[1]));
                UnityEngine.Debug.DrawLine(ToUnityVector(item.EdgePositions[1]), ToUnityVector(item.EdgePositions[1] - new Vector2(_colliderSize.X, 0)));
                UnityEngine.Debug.DrawLine(ToUnityVector(item.EdgePositions[1] - new Vector2(_colliderSize.X, 0)), ToUnityVector(item.EdgePositions[0]));

            }
        }

        private UnityEngine.Vector2 ToUnityVector(Vector2 vector)
        {
            return new UnityEngine.Vector2(vector.X, vector.Y);
        }
    }
}
