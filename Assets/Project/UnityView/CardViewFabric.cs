using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Project.UnityView
{
    internal class CardViewFabric : MonoBehaviour
    {
        [SerializeField] private CardViewInstance _viewTemplate;

        [SerializeField] private List<CardView> _cardViewList;

        [SerializeField] private Sprite _closedCardView;

        [SerializeField] private Transform _viewParent;

        private Dictionary<int, CardView> _cardViews;


        [System.Serializable]
        private struct CardView
        {
            public int Value;
            public List<Sprite> Sprites;
        }

        private void Awake()
        {
            _cardViews = _cardViewList.ToDictionary(x => x.Value);
        }

        public CardViewInstance CreateView(Interfaces.ICardModel model)
        {
            var cardViewInstance = GameObject.Instantiate(_viewTemplate, _viewParent);
            cardViewInstance.transform.SetAsFirstSibling();

            var spriteIndex = Random.Range(0, _cardViews[model.Value].Sprites.Count);

            cardViewInstance.Init(model, _cardViews[model.Value].Sprites[spriteIndex]);

            return cardViewInstance;
        }
    }
}
