using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

using static Project.Interfaces.ICardModel;

namespace Project.UnityView
{
    internal class CardViewInstance : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Image _image;

        private readonly int _closedRotaion = 180;
        private readonly int _openRotation = 0;

        private bool _gameActive;

        private float _moveTime = 0;
        private float _rotateTime = 0;

        public Interfaces.ICardModel Model { get; private set; }

        public void Init(Interfaces.ICardModel model, Sprite sprite)
        {
            _image.sprite = sprite;

            Model = model;

            Model.OnPositionChanged += OnPositionChanged;
            Model.OnStateChanged += OnStateChanged;
        }

        private void OnDestroy()
        {
            Model.OnPositionChanged -= OnPositionChanged;
            Model.OnStateChanged -= OnStateChanged;
        }

        public void ActivateGameState()
        {
            _gameActive = true;

            _moveTime = 1;
            _rotateTime = 0.5f;
        }

        private void OnStateChanged(State state)
        {
            switch (state)
            {
                case State.Close:
                    Rotate(_closedRotaion);
                    break;

                case State.Open:
                    Rotate(_openRotation);
                    break;

                default:
                    break;
            }
        }

        private void OnPositionChanged(System.Numerics.Vector2 position)
        {
            var unityScreenPosition = Camera.main.WorldToScreenPoint(new Vector2(position.X, position.Y));

            if(_gameActive)
            {
                transform.SetAsLastSibling();
            }
            
            transform.DOMove(unityScreenPosition, _moveTime);
        }

        private void Rotate(int resultAngle)
        {
            transform.DORotate(new Vector3(0, resultAngle, 0), _rotateTime);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log($"Clicked {transform.name}");
        }
    }
}
