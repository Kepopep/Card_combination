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
                    transform.DORotate(new Vector3(0, _closedRotaion, 0), 0.5f);
                    break;

                case State.Open:
                    transform.DORotate(new Vector3(0, _openRotation, 0), 0.5f);
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

            transform.DOMove(unityScreenPosition, 0);
        }
    }
}
