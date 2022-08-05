using UnityEngine;

namespace Project.UnityView.UnityHelper
{
    [ExecuteInEditMode]
    internal class CardLinksGenerator : MonoBehaviour
    {
        public CardLinksGenerator PreviousCard => _previous;
        public CardLinksGenerator NextCard => _next;

        [SerializeField] private CardLinksGenerator _previous;
        [SerializeField] private CardLinksGenerator _next;

        [SerializeField] private bool _inited;

        public void CreateChild(int index, int allCount, string baseName)
        {
            if (index == allCount)
            {
                return;
            }

            var gameObject = new GameObject($"{baseName} ({index})", typeof(RectTransform));
            var createdLinkedGenerator = gameObject.AddComponent<CardLinksGenerator>();
            _next = createdLinkedGenerator;

            createdLinkedGenerator.transform.SetParent(transform, false);
            createdLinkedGenerator.Init(this);

            index++;
            createdLinkedGenerator.CreateChild(index, allCount, baseName);

            return;
        }

        private void Init(CardLinksGenerator card)
        {
            if (_inited)
            {
                return;
            }

            _previous = card;
        }
    }
}