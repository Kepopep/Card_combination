using System;
using System.Numerics;

namespace Project.Interfaces
{
    public interface ICardModel
    {
        public enum State
        {
            Inactive,
            Active,
            Close,
            Open
        }

        public event Action<State> OnStateChanged;
        public event Action<Vector2> OnPositionChanged;

        public int Value { get; }
    }
}
