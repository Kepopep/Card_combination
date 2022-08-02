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
        private List<CardViewInstance> _createdViewInstances;


        [System.Serializable]
        private struct CardView
        {
            public int Value;
            public List<Sprite> Sprites;
        }

        private void Awake()
        {
            _cardViews = _cardViewList.ToDictionary(x => x.Value);
            _createdViewInstances = new List<CardViewInstance>();
        }

        public void CreateView(Interfaces.ICardModel model)
        {
            var cardView = GameObject.Instantiate(_viewTemplate, _viewParent);
            cardView.transform.SetAsFirstSibling();

            var spriteIndex = Random.Range(0, _cardViews[model.Value].Sprites.Count);

            cardView.Init(model, _cardViews[model.Value].Sprites[spriteIndex]);

            _createdViewInstances.Add(cardView);
        }

        public void Init()
        {
            foreach (var item in _createdViewInstances)
            {
                item.GameInited(); // TODO rename!!!!
            }
        }
    }
}
