using DG.Tweening;
using Project.Core;
using Project.Interfaces;
using Project.UnityView.UI;
using System.Collections.Generic;
using UnityEngine;

using static Project.Interfaces.ICardGame;

namespace Project.UnityView
{
    public class CardGameView : MonoBehaviour
    {
        [SerializeField] private FieldCardPatten _fieldCardLinker;
        [SerializeField] private CardViewFabric _cardViewFabric;
        [SerializeField] private StashCardsPivotHolder _stashCardsPivotHolder;
        [SerializeField] private CardGameUI _cardGameUI;

        private ICardGame _game;
        private Camera _camera;

        private List<CardViewInstance> _createdViewInstances;

        void Start()
        {
            _createdViewInstances = new List<CardViewInstance>();

            DOTween.Init();

            _camera = Camera.main;

            _game = new CardGame();

            _game.OnCardGameCardCreated += OnCardGameCardCreated; 
            _game.OnCardGameInited += OnCardGameInited;
            _game.OnCardGameEnd += OnCardGameEnd;

            _game.Init(_fieldCardLinker, _stashCardsPivotHolder);
            _game.StartGame();

            _cardGameUI.OnStartButtonClicked += OnStartButtonClicked;
        }

        private void OnStartButtonClicked()
        {
            _game.StartGame();
        }

        private void OnDestroy()
        {
            _game.OnCardGameCardCreated -= OnCardGameCardCreated;
            _game.OnCardGameInited -= OnCardGameInited;
        }

        private void Update()
        {
            _game.Update();

            if (Input.GetMouseButtonDown(0))
            {
                var worldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                _game.HandleInput(new System.Numerics.Vector2(worldPosition.x, worldPosition.y));
            }
        }

        private void OnCardGameCardCreated(List<ICardModel> cardModels)
        {
            DOTween.KillAll();

            foreach (var item in _createdViewInstances)
            {
                GameObject.Destroy(item.gameObject);
            }
            
            _createdViewInstances.Clear();

            foreach (var item in cardModels)
            {
                var createdView = _cardViewFabric.CreateView(item);

                _createdViewInstances.Add(createdView);
            }
        }

        private void OnCardGameInited()
        {
            foreach (var item in _createdViewInstances)
            {
                item.ActivateGameState();
            }
        }

        private void OnCardGameEnd(EndResult result)
        {
            switch (result)
            {
                case EndResult.Win:
                    _cardGameUI.ActivateWinScreen();
                    break;

                case EndResult.Fail:
                    _cardGameUI.ActivateFailScreen();
                    break;
            }
        }
    }
}