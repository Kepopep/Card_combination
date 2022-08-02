using Project.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

using static Project.Interfaces.ICardComboGenerator;

[ExecuteInEditMode]
public class FieldCardLinker : MonoBehaviour, ICardComboGenerator
{
    [SerializeField] private bool _update;

    [SerializeField] private FieldGenerationInfo _generationInfo;

    private void OnValidate()
    {
        if (!_update)
        {
            return;
        }

        //_generationInfo = new FieldGenerationInfo();

        var cardLinksGenerators = GetComponentsInChildren<CardLinksGenerator>();
        var childCount = cardLinksGenerators.Length;

        _generationInfo.CardViewLinkMediators = new List<CardViewLinkMediator>();

        var camera = Camera.main;

        for (int i = 0; i < childCount; i++)
        {
            var position = camera.ScreenToWorldPoint(cardLinksGenerators[i].transform.position);

            _generationInfo.CardViewLinkMediators.Add(new CardViewLinkMediator()
            {
                Index = i,
                PreviousIndex = Array.IndexOf(cardLinksGenerators, cardLinksGenerators[i].PreviousCard),
                NextIndex = Array.IndexOf(cardLinksGenerators, cardLinksGenerators[i].NextCard),
                Position = new Point(position.x, position.y)
            });
        }

        _generationInfo.CardCount = _generationInfo.CardViewLinkMediators.Count;

        var decks = GetComponentsInChildren<CardPositionGenerator>();
        _generationInfo.DeckSizes = new List<int>();

        foreach (var item in decks)
        {
            _generationInfo.DeckSizes.Add(item.GetComponentsInChildren<CardLinksGenerator>().Length);
        }

        _update = false;
    }

    public FieldGenerationInfo GetFieldGenerationInfo()
    {
        return _generationInfo;
    }
}
