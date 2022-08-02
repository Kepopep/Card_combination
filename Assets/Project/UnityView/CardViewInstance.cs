using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

using static Project.Interfaces.ICardModel;

namespace Project.UnityView
{
    public class CardViewInstance : MonoBehaviour
    {
        [SerializeField] private Image _image;

        private readonly int _closedRotaion = 180;
        private readonly int _openRotation = 0;
        private readonly float _moveTime = 1f;

        private bool _gameInited;

        public void Init(Interfaces.ICardModel model, Sprite sprite)
        {
            _image.sprite = sprite;

            model.OnPositionChanged += OnPositionChanged;
            model.OnStateChanged += OnStateChanged;
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

                case State.Inactive:
                    break;

                default:
                    break;
            }
        }

        private void OnPositionChanged(System.Numerics.Vector2 position)
        {
            var unityScreenPosition = Camera.main.WorldToScreenPoint(new Vector2(position.X, position.Y));

            if(_gameInited)
            {
                transform.SetAsLastSibling();
                transform.DOMove(unityScreenPosition, _moveTime);
            }
            else
            {
                transform.DOMove(unityScreenPosition, 0);
            }
        }

        private void Rotate(int resultAngle)
        {
            if (_gameInited)
            {
                transform.DORotate(new Vector3(0, resultAngle, 0), 0.5f);
            }
            else
            {
                transform.DORotate(new Vector3(0, resultAngle, 0), 0);
            }
        }

        public void GameInited()
        {
            _gameInited = true;
        }
    }
}
