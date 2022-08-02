using System.Collections.Generic;
using System.Numerics;

namespace Project.Interfaces
{
    internal interface IStashPresenter
    {
        public List<Vector2> GetStashPivots();
        public Vector2 GetHandPivot();
    }
}
