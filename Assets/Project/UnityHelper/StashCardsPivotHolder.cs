using Project.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class StashCardsPivotHolder : MonoBehaviour, IStashPresenter
{
    [SerializeField] private List<Transform> _pivots;

    public List<System.Numerics.Vector2> GetPivots()
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
