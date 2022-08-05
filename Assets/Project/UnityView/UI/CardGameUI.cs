using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

namespace Project.UnityView.UI
{
    internal class CardGameUI : MonoBehaviour
    {
        [Serializable]
        private struct ScreenText
        {
            public string Header;
            public string Button;
        }

        public event Action OnStartButtonClicked;

        [SerializeField] private CanvasGroup _canvasGroup;

        [Space]

        [SerializeField] private Button _button;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private TextMeshProUGUI _headerText;

        [Space]

        [SerializeField] private ScreenText _winText;
        [SerializeField] private ScreenText _loseText;

        [Space]

        [SerializeField] private float _resultWaitTime;
        [SerializeField] private float _fadeTime;

        private bool _isActive;

        private void Awake()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        private void Start()
        {
            _canvasGroup.DOFade(0, 0);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void ActivateWinScreen()
        {
            StartCoroutine(FaidIn(_winText));
        }

        public void ActivateFailScreen()
        {
            StartCoroutine(FaidIn(_loseText));
        }

        public void OnButtonClicked()
        {
            if(!_isActive)
            {
                return;
            }

            OnStartButtonClicked?.Invoke();

            FaidOut();
        }
   
        private IEnumerator FaidIn(ScreenText screenText)
        {
            gameObject.SetActive(true);

            _buttonText.text = screenText.Button;
            _headerText.text = screenText.Header;

            yield return new WaitForSeconds(_resultWaitTime);

            _canvasGroup.DOFade(0, 0);
            _canvasGroup.DOFade(1, _fadeTime);

            yield return new WaitForSeconds(_fadeTime);

            _isActive = true;

            yield return null;
        }

        private void FaidOut()
        {
            _isActive = false;

            _canvasGroup.DOFade(0, _fadeTime);
        }
    }
}
