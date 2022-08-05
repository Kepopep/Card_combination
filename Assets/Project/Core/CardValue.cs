using System;

namespace Project.Core
{
    public struct CardValue
    {
        // 0 - 12 = 2 - A

        public int Value;

        public static readonly int MaxValue = 12;

        public CardValue(int value)
        {
            Value = value;
        }

        public static CardValue operator +(CardValue cardValue, int addValue)
        {
            cardValue.Value = (cardValue.Value + addValue) % (MaxValue + 1);
            return cardValue;
        }

        public static CardValue operator -(CardValue cardValue, int addValue)
        {
            var tempValue = cardValue.Value - addValue;
            cardValue.Value = tempValue < 0 ? (MaxValue + 1) - (Math.Abs(tempValue) % (MaxValue + 1)) : tempValue;
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
