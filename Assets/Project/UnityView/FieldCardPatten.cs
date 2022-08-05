using Project.Interfaces;
using Project.UnityView.UnityHelper;
using System;
using System.Collections.Generic;
using UnityEngine;

using static Project.Interfaces.IFieldPatternGenerator;

namespace Project.UnityView
{
    [ExecuteInEditMode]
    public class FieldCardPatten : MonoBehaviour, IFieldPatternGenerator
    {
        [SerializeField] private bool _update;

        [SerializeField] private FieldPatternInfo _pattern;

        private void OnValidate()
        {
            if (!_update)
            {
                return;
            }

            var cardLinksGenerators = GetComponentsInChildren<CardLinksGenerator>();
            var childCount = cardLinksGenerators.Length;

            _pattern.CardViewLinkMediators = new List<CardViewLinkMediator>();

            var camera = Camera.main;

            for (int i = 0; i < childCount; i++)
            {
                var position = camera.ScreenToWorldPoint(cardLinksGenerators[i].transform.position);

                _pattern.CardViewLinkMediators.Add(new CardViewLinkMediator()
                {
                    Index = i,
                    PreviousIndex = Array.IndexOf(cardLinksGenerators, cardLinksGenerators[i].PreviousCard),
                    NextIndex = Array.IndexOf(cardLinksGenerators, cardLinksGenerators[i].NextCard),
                    Position = new Point(position.x, position.y)
                });
            }

            _pattern.CardCount = _pattern.CardViewLinkMediators.Count;

            var decks = GetComponentsInChildren<CardPositionGenerator>();
            _pattern.DeckSizes = new List<int>();

            foreach (var item in decks)
            {
                _pattern.DeckSizes.Add(item.GetComponentsInChildren<CardLinksGenerator>().Length);
            }

            _update = false;
        }

        public FieldPatternInfo GetFieldPatternInfo()
        {
            return _pattern;
        }
    }
}
