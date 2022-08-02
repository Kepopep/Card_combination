using System;
using UnityEngine;

[ExecuteInEditMode]
public class CardPositionGenerator : MonoBehaviour
{
    public int CardsCount => _objectCount;
    [SerializeField] private int _objectCount;
    [SerializeField] private bool _create;

    private struct CardLinkMediator
    {
        public int Index;
        public int PreviousIndex;
        public int NextIndex;
        public Vector2 Position;
    }
    
    private void OnValidate()
    {
        if (!_create)
        {
            return;
        }

        var baseName = transform.name;

        var gameObject = new GameObject($"{baseName} ({0})", typeof(RectTransform));
        gameObject.transform.SetParent(transform, false);

        var linkGenerator = gameObject.AddComponent<CardLinksGenerator>();
        linkGenerator.CreateChild(1, _objectCount, baseName);

        _create = false;
    }
}
