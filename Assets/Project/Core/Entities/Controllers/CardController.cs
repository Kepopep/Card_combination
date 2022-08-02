using Project.Core.Entities.Models;
using System;
using System.Collections.Generic;
using System.Numerics;

using static Project.Interfaces.ICardModel;

namespace Project.Core.Entities.Controllers
{
    internal class CardController
    {
        public CardModel Model { get; private set; }

        public CardValue CardValue => Model.CardValue;

        private List<CardController> _linkedCards;

        public event Action<CardController> OnInteract;
        public event Action<CardController> OnOpen;
        public event Action<CardController> OnDeactivated;
        public event Action<CardController, Vector2> OnPositionChanged;

        public CardController Previous => _previous;

        private CardController _next;
        private CardController _previous;


        public CardController(CardValue value)
        {
            Model = new CardModel(value);
            Model.OnStateChanged += _model_OnStateChanged;
            Model.OnPositionChanged += ModelPositionChanged;

            _linkedCards = new List<CardController>();
        }

        private void _model_OnStateChanged(State newState)
        {
            switch (newState)
            {
                case State.Open:
                    OnOpen?.Invoke(this);
                    break;
                case State.Inactive:
                    OnDeactivated?.Invoke(this);
                    break;
                default:
                    break;
            }
        }

        public void InitRelationship(CardController previous, CardController next)
        {
            _previous = previous;
            _next = next;
        }

        public void InitRelationship(List<CardController> linkedCards)
        {
            _linkedCards = linkedCards;

            foreach (var item in _linkedCards)
            {
                item.OnDeactivated += OnLinkedCardDeactivated;
            }
        }

        public void InitRelationship(CardController linkedCard)
        {
            _linkedCards.Add(linkedCard);
            linkedCard.OnDeactivated += OnLinkedCardDeactivated;
        }

        private void ModelPositionChanged(Vector2 position)
        {
            OnPositionChanged?.Invoke(this, position);
        }

        private void OnLinkedCardDeactivated(CardController cardController)
        {
            _linkedCards.Remove(cardController);

            if(_linkedCards.Count == 0)
            {
                Open();
            }
        }

        public void Open()
        {
            Model.ChangeState(State.Open);
        }

        public void Deactivate()
        {
            Model.ChangeState(State.Inactive);
        }

        public bool CheackInteractCondition(int value)
        {
            return Model.CardValue.CanUseLikeCombination(value);
        }

        public void Interact()
        {
            OnInteract?.Invoke(this);
        }

        public void SetPosition(Vector2 position)
        {
            Model.SetPosition(position);
        }
    }
}
