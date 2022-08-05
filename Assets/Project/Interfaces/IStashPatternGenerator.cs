using System.Collections.Generic;
using System.Numerics;

namespace Project.Interfaces
{
    public interface IStashPatternGenerator
    {
        public List<Vector2> GetStashPositions();
        public Vector2 GetHandPosition();
    }
}
