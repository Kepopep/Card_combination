using System;
using System.Collections.Generic;

namespace Project.Core
{
    internal partial class CardComboGenerator
    {
        public struct CardsValueType
        {
            public List<CardValue> Stash;
            public List<CardValue> Field;
        }

        public struct ComboGenerationInfo
        {
            public int CardCount;
            public List<int> DeckSizes;
            public int MinLenght;
            public int MaxLenght;
        }

        private ComboGenerationInfo _generationInfo;

        private readonly float _positiveDirectionChance = 0.65f;
        private readonly float _changeDirectionChance = 0.15f;

        public CardComboGenerator(ComboGenerationInfo generationInfo)
        {
            ChangeGenerationInfo(generationInfo);
        }

        public void ChangeGenerationInfo(ComboGenerationInfo generationInfo)
        {
            _generationInfo = generationInfo;
        }

        public CardsValueType GenerateCards()
        {
            var shufledDeckValue = new CardsValueType();
            shufledDeckValue.Stash = new List<CardValue>();
            shufledDeckValue.Field = new List<CardValue>();

            int combinationLength;
            
            var tempField = new List<List<CardValue>>();

            for (int i = 0; i < _generationInfo.DeckSizes.Count; i++)
            {
                tempField.Add(new List<CardValue>());
            }

            if(_generationInfo.MinLenght == 0 || _generationInfo.MaxLenght == 0)
            {
                throw new ArgumentException($"{_generationInfo.GetType()} init error: min == 0 || max == 0");
            }

            for (int registeredСards = 0; registeredСards < _generationInfo.CardCount;)
            {
                var combinationDirection = Utils.Utils.RandomFloat() < _positiveDirectionChance ? 1 : -1;
                var changeCombinationDirection = Utils.Utils.RandomFloat() < _changeDirectionChance;

                var currentCardValue = Utils.Utils.RandomInt(CardValue.MaxValue);

                shufledDeckValue.Stash.Add(new CardValue(currentCardValue));

                combinationLength = Utils.Utils.RandomInt(_generationInfo.MinLenght, _generationInfo.MaxLenght);
                var changeDirectionIndex = combinationLength / 2;

                for (int j = 0; j < combinationLength; j++)
                {
                    var insertIndex = FindPosibleToInsertIndex(tempField);

                    if (insertIndex == -1)
                    {
                        break;
                    }
                        
                    if (changeCombinationDirection && j == changeDirectionIndex)
                    {
                        combinationDirection *= -1;
                    }

                    currentCardValue += combinationDirection;
                    currentCardValue = currentCardValue < 0 ? CardValue.MaxValue : currentCardValue;
                    currentCardValue = currentCardValue > CardValue.MaxValue ? 0 : currentCardValue;

                    tempField[insertIndex].Add(new CardValue(currentCardValue));

                    registeredСards++;
                }
            }

            foreach (var item in tempField)
            {
                shufledDeckValue.Field.AddRange(item);
            }

            return shufledDeckValue;
        }

        private int FindPosibleToInsertIndex(List<List<CardValue>> field)
        {
            var posibleToInsertIndexes = new List<int>();

            for (int i = 0; i < field.Count; i++)
            {
                if(field[i].Count < _generationInfo.DeckSizes[i])
                {
                    posibleToInsertIndexes.Add(i);
                }
            }

            return posibleToInsertIndexes.Count > 0 ? posibleToInsertIndexes[Utils.Utils.RandomInt(posibleToInsertIndexes.Count - 1)] : -1;
        }
    }
}
