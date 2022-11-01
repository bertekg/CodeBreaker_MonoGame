using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CodeBreaker_MonoGame.Scene
{
    internal class FinishScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _otherFont;
        private readonly Lang _lang;
        private readonly Color _endGameColor;
        private readonly string _endGameInfo, _timeText, _attemptsText, _correctCodeString;
        public FinishScene(Game1 game1, GameLogic gameLogic, SaveData saveData, SpriteFont bigFont, SpriteFont middleFont, Lang lang, double time)
        {
            _game1 = game1;
            _titleFont = bigFont;
            _otherFont = middleFont;
            _lang = lang;

            if (gameLogic.codeGuessed)
            {
                _endGameInfo = _lang.GetLangText(LangKey.FinishWin); _endGameColor = Color.Green;
            }
            else
            {
                _endGameInfo = _lang.GetLangText(LangKey.FinishLost); _endGameColor = Color.Red;
            }

            if (saveData.isTimeLimit)
            {
                _timeText = string.Format(_lang.GetLangText(LangKey.FinishRemainingTime), time);
            }
            else
            {
                _timeText = string.Format(_lang.GetLangText(LangKey.FinishPlayingTime), time);
            }

            if (saveData.isAttemptsLimit)
            {
                _attemptsText = _lang.GetLangText(LangKey.FinishRemainingAttempts) + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString();
            }
            else
            {
                _attemptsText = _lang.GetLangText(LangKey.FinishNumberOfAttempts) + gameLogic.numberOfAttempts.ToString();
            }
            _correctCodeString = _lang.GetLangText(LangKey.FinishCorrectCode) + gameLogic.CorrectCodeString();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _endGameInfo, new Vector2(200, 40), _endGameColor);

            spriteBatch.DrawString(_otherFont, _timeText, new Vector2(200, 100), Color.Black);

            spriteBatch.DrawString(_otherFont, _attemptsText, new Vector2(200, 160), Color.Black);

            spriteBatch.DrawString(_otherFont, _correctCodeString, new Vector2(200, 220), Color.Black);

            spriteBatch.DrawString(_otherFont, _lang.GetLangText(LangKey.FinishPlayAgain), new Vector2(10, 360), Color.Black);
            spriteBatch.DrawString(_otherFont, _lang.GetLangText(LangKey.GoBackMenu), new Vector2(10, 400), Color.Black);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                _game1.GoToGame();

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                _game1.GoToMainMenu(true);
        }
    }
}
