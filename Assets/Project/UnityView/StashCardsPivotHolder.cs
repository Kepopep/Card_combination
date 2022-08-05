using Project.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UnityView
{
    public class StashCardsPivotHolder : MonoBehaviour, IStashPatternGenerator
    {
        [SerializeField] private List<Transform> _pivots;
        [SerializeField] private Transform _handPivot;

        public System.Numerics.Vector2 GetHandPosition()
        {
            var position = Camera.main.ScreenToWorldPoint(_handPivot.position);

            return new System.Numerics.Vector2(position.x, position.y);
        }

        public List<System.Numerics.Vector2> GetStashPositions()
        {
            var result = new List<System.Numerics.Vector2>();
            var camera = Camera.main;

            foreach (var item in _pivots)
            {
                var position = camera.ScreenToWorldPoint(item.position);

                result.Add(new System.Numerics.Vector2(position.x, position.y));
            }

            return result;
        }
    }
}
