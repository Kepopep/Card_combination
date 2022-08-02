using System.Collections.Generic;
using System.Numerics;

namespace Project.Interfaces
{
    public interface ICardComboGenerator
    {
        [System.Serializable]
        public struct FieldGenerationInfo
        {
            public int CardCount;
            public List<int> DeckSizes;
            public int CombinationMinLenght;
            public int CombinationMaxLenght;

            public List<CardViewLinkMediator> CardViewLinkMediators;
        }

        [System.Serializable]
        public struct CardViewLinkMediator
        {
            public int Index;
            public int PreviousIndex;
            public int NextIndex;
            public Point Position;
        }

        [System.Serializable]
        public struct Point
        {
            public float X;
            public float Y;

            public Point(float x, float y)
            {
                X = x;
                Y = y;
            }
        }

        public FieldGenerationInfo GetFieldGenerationInfo();
    }
}
