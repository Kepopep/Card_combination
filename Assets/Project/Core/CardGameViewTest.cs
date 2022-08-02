using DG.Tweening;
using Project.Core;
using Project.UnityView;
using UnityEngine;

namespace Project.UnityView
{

    public class CardGameViewTest : MonoBehaviour
    {
        [SerializeField] private FieldCardLinker _fieldCardLinker;
        [SerializeField] private CardViewFabric _cardViewFabric;
        [SerializeField] private StashCardsPivotHolder _stashCardsPivotHolder;

        private CardGame _game;

        void Start()
        {
            DOTween.Init();

            _game = new CardGame();
            _game.OnCardCreated += _game_OnCardCreated;

            _game.Init(_fieldCardLinker, _stashCardsPivotHolder);
        }

        private void _game_OnCardCreated(Project.Interfaces.ICardModel model)
        {
            _cardViewFabric.CreateView(model);
        }

        private void Update()
        {
            _game.Update();

            if (Input.GetMouseButtonDown(0)) //TODO replase with input system
            {
                var worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _game.HandleClick(new System.Numerics.Vector2(worldPosition.x, worldPosition.y));
            }
        }
    }
}