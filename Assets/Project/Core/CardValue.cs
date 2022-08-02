namespace Project.Core
{
    public struct CardValue
    {
        // 0 - 12 = 2 - A

        public int Value;

        public CardValue(int value)
        {
            Value = value;
        }

        public static CardValue operator +(CardValue cardValue, int addValue)
        {
            cardValue.Value = (cardValue.Value + addValue) % 12;
            return cardValue;
        }

        public static CardValue operator -(CardValue cardValue, int addValue)
        {
            var tempValue = cardValue.Value - addValue;
            cardValue.Value = tempValue < 0 ? 12 - (tempValue % 12) : tempValue;
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
