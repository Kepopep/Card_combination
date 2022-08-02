using Project.Core.Entities.Controllers;
using System;
using System.Collections.Generic;

namespace Project.Core
{
    internal partial class CardShuffler
    {
        public struct ShufledCardValue
        {
            public List<CardValue> Stash;
            public List<CardValue> Field;
        }

        public struct CardGenerationInfo
        {
            public int CardCount;
            public List<int> DeckSizes;
            public int CombinationMinLenght;
            public int CombinationMaxLenght;
        }

        private CardGenerationInfo _generationInfo;

        private Random _random = new Random();


        public CardShuffler(CardGenerationInfo generationInfo)
        {
            _generationInfo = generationInfo;
        }

        public ShufledCardValue GenerateCards()
        {
            var shufledDeckValue = new ShufledCardValue();
            shufledDeckValue.Stash = new List<CardValue>();
            shufledDeckValue.Field = new List<CardValue>();

            int combinationLength;
            
            
            var tempField = new List<List<CardValue>>();

            for (int i = 0; i < _generationInfo.DeckSizes.Count; i++)
            {
                tempField.Add(new List<CardValue>());
            }

            if(_generationInfo.CombinationMinLenght == 0 || _generationInfo.CombinationMaxLenght == 0)
            {
                throw new ArgumentException($"{_generationInfo.GetType()} init error");
            }

            for (int registeredСards = 0; registeredСards < _generationInfo.CardCount;)
            {
                var combinationDirection = RandomFloat() < 0.65f ? 1 : -1;
                var changeCombinationDirection = RandomFloat() < 0.15f;

                var currentCardValue = RandomInt(12);

                shufledDeckValue.Stash.Add(new CardValue(currentCardValue));

                combinationLength = RandomInt(_generationInfo.CombinationMinLenght, _generationInfo.CombinationMaxLenght);
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
                    currentCardValue = currentCardValue < 0 ? 12 : currentCardValue;
                    currentCardValue = currentCardValue > 12 ? 0 : currentCardValue;

                    tempField[insertIndex].Add(new CardValue(currentCardValue));

                    registeredСards++;
                }
            }

            foreach (var item in tempField)
            {
                shufledDeckValue.Field.AddRange(item);
            }

            DebugLog(shufledDeckValue);

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

            return posibleToInsertIndexes.Count > 0 ? posibleToInsertIndexes[RandomInt(posibleToInsertIndexes.Count - 1)] : -1;
        }

        private void DebugLog(ShufledCardValue shufledCards)
        {
            foreach (var item in shufledCards.Stash)
            {
                UnityEngine.Debug.Log($"hand: {item.Value}");
            }

            foreach (var item in shufledCards.Field)
            {
                UnityEngine.Debug.Log($"combo {item.Value}:");
            }

            UnityEngine.Debug.Log($"all cards count {shufledCards.Field.Count}:");
        }

        private int RandomInt(int fromIncluded, int toIncluded) // TODO replase
        {
            return _random.Next(fromIncluded, toIncluded + 1);
        }

        private int RandomInt(int toIncluded)
        {
            return _random.Next(toIncluded + 1);
        }

        private float RandomFloat()
        {
            return (float)_random.NextDouble();
        }
    }
}
