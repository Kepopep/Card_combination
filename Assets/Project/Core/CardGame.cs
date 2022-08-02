using Project.Core.Deck;
using Project.Core.Entities.Controllers;
using Project.Interaction;
using Project.Interfaces;
using System;
using System.Collections.Generic;
using System.Numerics;

using static Project.Core.CardShuffler;
using static Project.Interfaces.ICardComboGenerator;

namespace Project.Core
{
    internal class CardGame
    {
        private InteractionSystem _cardsInteractionSystem;

        private FieldDeck _fieldDeck;
        private StashDeck _stashDeck;

        public CardController CurrentCard { get; private set; }

        public event Action<ICardModel> OnCardCreated;
        public event Action OnCardGameInited;

        private ICardComboGenerator _fieldInfoGenerator;
        private IStashPresenter _stashPresenter;

        private Vector2 _handPosition;

        public FieldGenerationInfo FieldGenerationInfo { get; private set; }

        public void Init(ICardComboGenerator comboGenerator, IStashPresenter stashPresenter)
        {
            _cardsInteractionSystem = new InteractionSystem();
            _cardsInteractionSystem.OnClick += OnCardClick;

            _fieldInfoGenerator = comboGenerator;
            _stashPresenter = stashPresenter;

            _handPosition = _stashPresenter.GetHandPivot();

            FieldGenerationInfo = _fieldInfoGenerator.GetFieldGenerationInfo();

            var generationInfo = new CardGenerationInfo() //TODO think about naming
            {
                CardCount = FieldGenerationInfo.CardCount,
                DeckSizes = FieldGenerationInfo.DeckSizes,
                CombinationMinLenght = FieldGenerationInfo.CombinationMinLenght,
                CombinationMaxLenght = FieldGenerationInfo.CombinationMaxLenght
            };

            var cardShuffler = new CardShuffler(generationInfo);
            var shafledCards = cardShuffler.GenerateCards();

            var stashCards = CreateCardControllers(shafledCards.Stash);
            _stashDeck = new StashDeck(stashCards, this, _stashPresenter.GetStashPivots());
            _stashDeck.Init();

            var fieldCards = CreateCardControllers(shafledCards.Field);
            _fieldDeck = new FieldDeck(fieldCards, this);
            _fieldDeck.Init();
            
            _cardsInteractionSystem.Init(fieldCards);
            _cardsInteractionSystem.Init(stashCards);

            OnCardGameInited?.Invoke();
        }

        private void OnCardClick(CardController cardController)
        {
            cardController.Interact();
            UnityEngine.Debug.Log($"Clicked, current points {CurrentCard.Model.CardValue.Value}, clicked card {cardController.CardValue.Value}");
        }

        public void HandleClick(Vector2 clickPoint)
        {
            _cardsInteractionSystem.Click(clickPoint);
        }

        public void Update()
        {
            _cardsInteractionSystem.Update();
        }

        public void ChangeCurrentCard(CardController cardController)
        {
            cardController.Deactivate();
            cardController.SetPosition(_handPosition);

            CurrentCard = cardController;
        }

        private List<CardController> CreateCardControllers(List<CardValue> cardValues)
        {
            var result = new List<CardController>();

            foreach (var item in cardValues)
            {
                var controller = new CardController(item);

                result.Add(controller);
                OnCardCreated?.Invoke(controller.Model);
            }

            return result;
        }
    }
}
