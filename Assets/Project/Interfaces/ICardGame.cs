using System;
using System.Collections.Generic;
using System.Numerics;

namespace Project.Interfaces
{
    public interface ICardGame
    {
        public enum EndResult
        {
            Win,
            Fail
        }

        public event Action<List<ICardModel>> OnCardGameCardCreated;
        public event Action OnCardGameInited;

        public event Action<EndResult> OnCardGameEnd;

        public void Init(IFieldPatternGenerator comboGenerator, IStashPatternGenerator stashPresenter);
        public void StartGame();

        public void HandleInput(Vector2 clickPoint);
        public void Update();
    }
}
