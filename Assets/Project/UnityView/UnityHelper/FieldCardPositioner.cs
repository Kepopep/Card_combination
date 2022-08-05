using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.UnityView.UnityHelper
{
    [ExecuteInEditMode]
    public class FieldCardPositioner : MonoBehaviour
    {
        [SerializeField] private bool _update;

        [SerializeField] private List<Vector2> _plasePattern;

        private List<CardLinksGenerator> _positionGenerators = new List<CardLinksGenerator>();


        private void OnValidate()
        {
            if(!_update)
            {
                return;
            }

            _positionGenerators = GetComponentsInChildren<CardLinksGenerator>().ToList();

            var count = _positionGenerators.Count - 1;

            for (int i = 0; i < count; i++)
            {
                var item = _positionGenerators[i + 1].transform;
                var itemPatternPosition = _plasePattern[i];

                item.localPosition = new Vector3(itemPatternPosition.x, itemPatternPosition.y);
            }

            _update = false;
        }

    }
}
