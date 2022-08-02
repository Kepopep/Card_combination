using Project.Interfaces;
using System;
using System.Numerics;

using static Project.Interfaces.ICardModel;

namespace Project.Core.Entities.Models
{
    internal class CardModel : ICardModel
    {
        public CardValue CardValue { get; private set; }
        public Vector2 Position { get; private set; }
        public State State { get; private set; } 

        public int Value => CardValue.Value;

        public event Action<State> OnStateChanged;
        public event Action<Vector2> OnPositionChanged;

        public CardModel(CardValue value)
        {
            CardValue = value;
        }

        public void ChangeState(State newState)
        {
            State = newState;

            OnStateChanged?.Invoke(State);
        }

        public void SetPosition(Vector2 newPosition)
        {
            Position = newPosition;
            OnPositionChanged?.Invoke(Position);
        }
    }
}
