using Project.Core.Deck;

using static Project.Interfaces.ICardGame;

namespace Project.Core
{
    internal class ResultChecker
    {
        private CardGame _game;

        private FieldDeck _fieldDeck;
        private StashDeck _stashDeck;

        public ResultChecker(CardGame game, FieldDeck fieldDeck, StashDeck stashDeck)
        {
            _game = game;

            _fieldDeck = fieldDeck;
            _fieldDeck.OnCardsCountUpdated += CheckGameState;

            _stashDeck = stashDeck;
            _stashDeck.OnCardsCountUpdated += CheckGameState;
        }
        
        ~ResultChecker()
        {
            _fieldDeck.OnCardsCountUpdated -= CheckGameState;
            _stashDeck.OnCardsCountUpdated -= CheckGameState;
        }

        private void CheckGameState()
        {
            if(!_game.GameIsActive)
            {
                return;
            }

            if (_fieldDeck.CardsCount == 0)
            {
                _game.EndGame(EndResult.Win);
                return;
            }

            if(_fieldDeck.CardsCount != 0 && _stashDeck.CardsCount == 0 && !_fieldDeck.CheckInteractPossibility())
            {
                _game.EndGame(EndResult.Fail);
                return;
            }
        }
    }
}
