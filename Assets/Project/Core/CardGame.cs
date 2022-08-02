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
        public event Action<CardController> OnCurrentCardChanged;

        public event Action<ICardModel> OnCardCreated;

        private ICardComboGenerator _fieldInfoGenerator;
        private IStashPresenter _stashPresenter;

        public void Init(ICardComboGenerator comboGenerator, IStashPresenter stashPresenter)
        {
            _cardsInteractionSystem = new InteractionSystem();
            _cardsInteractionSystem.OnClick += OnCardClick;

            _fieldInfoGenerator = comboGenerator;
            _stashPresenter = stashPresenter;

            var fieldGenerationInfo = _fieldInfoGenerator.GetFieldGenerationInfo();

            var generationInfo = new CardGenerationInfo()
            {
                CardCount = fieldGenerationInfo.CardCount,
                DeckSizes = fieldGenerationInfo.DeckSizes,
                CombinationMinLenght = fieldGenerationInfo.CombinationMinLenght,
                CombinationMaxLenght = fieldGenerationInfo.CombinationMaxLenght
            };

            var cardShuffler = new CardShuffler(generationInfo);
            var shafledCards = cardShuffler.GenerateCards();

            var fieldCards = CreateCardControllers(shafledCards.Field);
            InitFieldCards(fieldCards, fieldGenerationInfo);

            var firstCards = fieldCards.FindAll(x => x.Previous == null);

            foreach (var item in firstCards)
            {
                item.Open();
            }

            var stashCards = CreateCardControllers(shafledCards.Stash);
            InitStashCards(stashCards, _stashPresenter.GetPivots());

            _stashDeck = new StashDeck(stashCards, this);
            
            _cardsInteractionSystem.Init(fieldCards);
            _cardsInteractionSystem.Init(stashCards);
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
            _cardsInteractionSystem.DrawDebug();
        }

        public void ChangeCurrentCard(CardController cardController)
        {
            CurrentCard = cardController;

            OnCurrentCardChanged?.Invoke(cardController);
        }

        private void InitFieldCards(List<CardController> cardControllers, FieldGenerationInfo cartPattern)
        {
            var cardCount = cardControllers.Count;

            int previousIndex;
            int nextIndex;

            CardController previousCard;
            CardController nextCard;

            Vector2 position;

            for (int i = 0; i < cardCount; i++)
            {
                previousIndex = cartPattern.CardViewLinkMediators[i].PreviousIndex;
                nextIndex = cartPattern.CardViewLinkMediators[i].NextIndex;

                previousCard = previousIndex == -1 ? null : cardControllers[previousIndex];
                nextCard = nextIndex == -1 ? null : cardControllers[nextIndex];

                cardControllers[i].InitRelationship(previousCard, nextCard);

                position.X = cartPattern.CardViewLinkMediators[i].Position.X;
                position.Y = cartPattern.CardViewLinkMediators[i].Position.Y;
                cardControllers[i].SetPosition(position);
            }
        }

        private void InitStashCards(List<CardController> cardControllers, List<Vector2> positions)
        {
            var positionIndex = 0;
            var positionsCount = positions.Count;

            foreach (var item in cardControllers)
            {
                item.SetPosition(positions[positionIndex]);

                positionIndex = positionIndex + 1 < positionsCount ? positionIndex + 1 : positionsCount - 1; // TODO clear
            }
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
