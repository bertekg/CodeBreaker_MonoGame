using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Control;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame.Scene
{
    internal class FinishScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _otherFont, _navigationFont;
        private readonly Lang _lang;
        private readonly Color _endGameColor;
        private readonly string _textGameResult, _textTime, _textAttempts, _textCode, _textPlayAgainToolTip,
            _textGoGameModToolTip, _textGoMainMenuToolTip;
        private readonly Vector2 _locGameResult, _locTime, _locAttempts, _locCode, _locPlayAgainToolTip,
            _locGoGameModToolTip, _locGoMainMenuToolTip;
        private List<Component> _gameComponents;
        public FinishScene(Game1 game1, GameLogic gameLogic, SaveData saveData, SpriteFont bigFont, SpriteFont middleFont,
            SpriteFont smallFont, Lang lang, double time, Texture2D buttonNavigation)
        {
            _game1 = game1;
            _titleFont = bigFont;
            _otherFont = middleFont;
            _navigationFont = smallFont;
            _lang = lang;

            if (gameLogic.codeGuessed)
            {
                _textGameResult = _lang.GetLangText(LangKey.FinishWin); _endGameColor = Color.Green;
            }
            else
            {
                _textGameResult = _lang.GetLangText(LangKey.FinishLost); _endGameColor = Color.Red;
            }
            _locGameResult = new Vector2((Game1.ScreenWidth / 2) - (_titleFont.MeasureString(_textGameResult).X / 2), 30);

            if (saveData.isTimeLimit)
            {
                _textTime = string.Format(_lang.GetLangText(LangKey.FinishRemainingTime), time);
            }
            else
            {
                _textTime = string.Format(_lang.GetLangText(LangKey.FinishPlayingTime), time);
            }
            _locTime = new Vector2((Game1.ScreenWidth / 2) - (_otherFont.MeasureString(_textTime).X / 2), 100);

            if (saveData.isAttemptsLimit)
            {
                _textAttempts = _lang.GetLangText(LangKey.FinishRemainingAttempts) + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString();
            }
            else
            {
                _textAttempts = _lang.GetLangText(LangKey.FinishNumberOfAttempts) + gameLogic.numberOfAttempts.ToString();
            }
            _locAttempts = new Vector2((Game1.ScreenWidth / 2) - (_otherFont.MeasureString(_textAttempts).X / 2), 160);

            _textCode = _lang.GetLangText(LangKey.FinishCorrectCode) + gameLogic.CorrectCodeString();
            _locCode = new Vector2((Game1.ScreenWidth / 2) - (_otherFont.MeasureString(_textCode).X / 2), 220);

            Button playAgainButon = new Button(buttonNavigation, smallFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonNavigation.Width / 2), 310),
                Text = _lang.GetLangText(LangKey.PlayAgain)
            };
            playAgainButon.Click += PlayAgain_Click;
            _textPlayAgainToolTip = _lang.GetLangText(LangKey.FinishPlayAgain);
            _locPlayAgainToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_textPlayAgainToolTip).X / 2)), 340);

            Button modGameButton = new Button(buttonNavigation, smallFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonNavigation.Width / 2), 390),
                Text = _lang.GetLangText(LangKey.Modifiers)
            };
            modGameButton.Click += GoModGame_Click;
            _textGoGameModToolTip = _lang.GetLangText(LangKey.ModifiersKey);
            _locGoGameModToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_textGoGameModToolTip).X / 2)), 420);

            Button mainMenuButton = new Button(buttonNavigation, smallFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonNavigation.Width / 2), 470),
                Text = _lang.GetLangText(LangKey.MainMenu)
            };
            mainMenuButton.Click += GoMainMenu_Click;
            _textGoMainMenuToolTip = _lang.GetLangText(LangKey.GoBackMenu);
            _locGoMainMenuToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_textGoMainMenuToolTip).X / 2)), 500);

            _gameComponents = new List<Component>()
            {
                playAgainButon, modGameButton, mainMenuButton
            };
        }
        private void PlayAgain_Click(object sender, EventArgs e)
        {
            PlayAgain();
        }
        private void GoModGame_Click(object sender, EventArgs e)
        {
            GoToModifiers();
        }
        private void GoMainMenu_Click(object sender, EventArgs e)
        {
            GoToMainMenu();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _textGameResult, _locGameResult, _endGameColor);

            spriteBatch.DrawString(_otherFont, _textTime, _locTime, Color.Black);
            spriteBatch.DrawString(_otherFont, _textAttempts, _locAttempts, Color.Black);
            spriteBatch.DrawString(_otherFont, _textCode, _locCode, Color.Black);

            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _textPlayAgainToolTip, _locPlayAgainToolTip, Color.Black);
            spriteBatch.DrawString(_navigationFont, _textGoGameModToolTip, _locGoGameModToolTip, Color.Black);
            spriteBatch.DrawString(_navigationFont, _textGoMainMenuToolTip, _locGoMainMenuToolTip, Color.Black);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (var component in _gameComponents)
                component.Update();

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                PlayAgain();

            if (keyboardState.IsKeyDown(Keys.M) || gamePadState.Buttons.Y == ButtonState.Pressed)
                GoToModifiers();

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                GoToMainMenu();
        }
        private void PlayAgain()
        {
            _game1.GoToGame();
        }
        private void GoToModifiers()
        {
            _game1.GoToModifiers();
        }
        private void GoToMainMenu()
        {
            _game1.GoToMainMenu(true);
        }
    }
}
