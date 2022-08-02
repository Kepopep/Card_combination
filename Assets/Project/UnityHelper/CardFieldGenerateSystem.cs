using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[ExecuteInEditMode]
public class CardFieldGenerateSystem : MonoBehaviour
{
    private List<CardPositionGenerator> _positionGenerators = new List<CardPositionGenerator>();
    private int _totalCardsCount;

    private List<Queue<CardLinksGenerator>> _cardOrder = new List<Queue<CardLinksGenerator>>();

    private void OnEnable()
    {
        _positionGenerators = GetComponentsInChildren<CardPositionGenerator>().ToList();

        foreach (var item in _positionGenerators)
        {
            _cardOrder.Add(new Queue<CardLinksGenerator>());
            _totalCardsCount += item.CardsCount;
        }

        _totalCardsCount = 0;
    }
}
