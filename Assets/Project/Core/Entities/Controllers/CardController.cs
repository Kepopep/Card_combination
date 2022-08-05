using Project.Core.Entities.Models;
using System;
using System.Numerics;

using static Project.Interfaces.ICardModel;

namespace Project.Core.Entities.Controllers
{
    internal class CardController
    {
        public CardModel Model { get; private set; }

        public CardValue CardValue => Model.CardValue;

        public event Action<CardController> OnInteract;

        public event Action<CardController> OnOpen;

        public event Action<CardController> OnActivated;
        public event Action<CardController> OnDeactivated;

        public event Action<CardController, Vector2> OnPositionChanged;

        public CardController Previous => _previous;
        public CardController Next => _next;

        private CardController _next;
        private CardController _previous;


        public CardController(CardValue value)
        {
            Model = new CardModel(value);
            Model.OnStateChanged += TriggerOnStateChanged;
            Model.OnPositionChanged += TriggerOnPositionChanged;
        }

        ~CardController()
        {
            Model.OnStateChanged -= TriggerOnStateChanged;
            Model.OnPositionChanged -= TriggerOnPositionChanged;
        }

        public void InitRelationship(CardController previous, CardController next)
        {
            _previous = previous;
            _next = next;
        }
        
        public void Open()
        {
            Model.ChangeState(State.Open);
        }

        public void ActivateInteractions()
        {
            Model.ChangeState(State.Active);
        }

        public void DeactivateInteractions()
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

        private void TriggerOnPositionChanged(Vector2 position)
        {
            OnPositionChanged?.Invoke(this, position);
        }
        
        private void TriggerOnStateChanged(State newState)
        {
            switch (newState)
            {
                case State.Open:
                    OnOpen?.Invoke(this);
                    break;

                case State.Active:
                    OnActivated?.Invoke(this);
                    break;

                case State.Inactive:
                    OnDeactivated?.Invoke(this);
                    break;

                default:
                    break;
            }
        }
    }
}
