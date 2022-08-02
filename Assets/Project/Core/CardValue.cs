using System;

namespace Project.Core
{
    public struct CardValue
    {
        // 0 - 12 = 2 - A

        public int Value;

        private static readonly int _allCardCount = 13;

        public CardValue(int value)
        {
            Value = value;
        }

        public static CardValue operator +(CardValue cardValue, int addValue)
        {
            cardValue.Value = (cardValue.Value + addValue) % _allCardCount;
            return cardValue;
        }

        public static CardValue operator -(CardValue cardValue, int addValue)
        {
            var tempValue = cardValue.Value - addValue;
            cardValue.Value = tempValue < 0 ? _allCardCount - (Math.Abs(tempValue) % _allCardCount) : tempValue;
            return cardValue;
        }
            
        public bool CanUseLikeCombination(int otherValue)
        {
            var plusValue = this + 1;
            var minusValue = this - 1;

            return plusValue.Value == otherValue || minusValue.Value == otherValue;
        }

    }
}
