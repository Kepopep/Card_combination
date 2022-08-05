using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;
using Project.Interfaces;
using Project.Interaction;
using Project.Core.Deck;
using Project.Core.Entities.Controllers;

using static Project.Core.CardComboGenerator;
using static Project.Interfaces.IFieldPatternGenerator;
using static Project.Interfaces.ICardGame;

namespace Project.Core
{
    public class CardGame : ICardGame
    {
        public FieldPatternInfo FieldPattern { get; private set; }
        internal CardController CurrentCard { get; private set; }
        public bool GameIsActive { get; private set; }

        public event Action<List<ICardModel>> OnCardGameCardCreated;
        public event Action OnCardGameInited;
        public event Action<EndResult> OnCardGameEnd;

        private InteractionSystem _cardsInteractionSystem;

        private FieldDeck _fieldDeck;
        private StashDeck _stashDeck;

        private IFieldPatternGenerator _fieldPatternGenerator;
        private IStashPatternGenerator _stashPatternGenerator;

        private Vector2 _handPosition;

        private ResultChecker _gameLoop;

        private CardComboGenerator _cardShuffler;

        public void Init(IFieldPatternGenerator comboGenerator, IStashPatternGenerator stashPresenter)
        {
            _cardsInteractionSystem = new InteractionSystem();
            _cardsInteractionSystem.OnClick += OnCardClick;

            _fieldPatternGenerator = comboGenerator;
            _stashPatternGenerator = stashPresenter;

            _handPosition = _stashPatternGenerator.GetHandPosition();

            FieldPattern = _fieldPatternGenerator.GetFieldPatternInfo();

            var generationInfo = new ComboGenerationInfo()
            {
                CardCount = FieldPattern.CardCount,
                DeckSizes = FieldPattern.DeckSizes,
                MinLenght = FieldPattern.CombinationMinLenght,
                MaxLenght = FieldPattern.CombinationMaxLenght
            };

            _cardShuffler = new CardComboGenerator(generationInfo);

            _fieldDeck = new FieldDeck(this);
            _stashDeck = new StashDeck(this, _stashPatternGenerator.GetStashPositions());

            _gameLoop = new ResultChecker(this, _fieldDeck, _stashDeck);
        }

        public void StartGame()
        {
            var shafledCards = _cardShuffler.GenerateCards();

            var stashCards = CreateCardControllers(shafledCards.Stash);
            var fieldCards = CreateCardControllers(shafledCards.Field);

            var cardModels = new List<ICardModel>();

            cardModels.AddRange(stashCards.Select(x => (ICardModel)x.Model));
            cardModels.AddRange(fieldCards.Select(x => (ICardModel)x.Model));

            OnCardGameCardCreated?.Invoke(cardModels);

            _stashDeck.Init(stashCards);
            _fieldDeck.Init(fieldCards);

            _cardsInteractionSystem.Reset();
            _cardsInteractionSystem.InitInteraction(stashCards);
            _cardsInteractionSystem.InitInteraction(fieldCards);

            GameIsActive = true;

            OnCardGameInited?.Invoke();
        }

        public void HandleInput(Vector2 clickPoint)
        {
            _cardsInteractionSystem.Click(clickPoint);
        }

        public void Update()
        {
            _cardsInteractionSystem.Update();
        }

        internal void EndGame(EndResult result)
        {
            GameIsActive = false;

            OnCardGameEnd?.Invoke(result);
        }

        internal void ChangeCurrentCard(CardController cardController)
        {
            cardController.DeactivateInteractions();
            cardController.SetPosition(_handPosition);

            CurrentCard = cardController;
        }

        private void OnCardClick(CardController cardController)
        {
            if(GameIsActive)
            {
                cardController.Interact();
            }
        }
      
        private List<CardController> CreateCardControllers(List<CardValue> cardValues)
        {
            var result = new List<CardController>();

            result = cardValues.Select(x => new CardController(x)).ToList();

            return result;
        }
    }
}
