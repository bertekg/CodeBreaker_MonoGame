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
    internal class InstructionScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _instructionFont, _navigationFont;
        private readonly Lang _lang;
        private Vector2 _locTitle, _locBackToolTip;
        private List<Component> _gameComponents;
        public InstructionScene(Game1 game1, SpriteFont titleFont, SpriteFont instructionFont, SpriteFont navigationFont,
            Lang lang, Texture2D buttonNavigation)
        {
            _game1 = game1;
            _titleFont = titleFont;
            _instructionFont = instructionFont;
            _navigationFont = navigationFont;
            _lang = lang;
            _locTitle = new Vector2((Game1.ScreenWidth / 2) -
                (_titleFont.MeasureString(_lang.GetLangText(LangKey.GameInstuction)).X / 2), 30);

            Button goMenuButton = new Button(buttonNavigation, _navigationFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (buttonNavigation.Width / 2), 470),
                Text = _lang.GetLangText(LangKey.Back)
            };
            goMenuButton.Click += GoMenuButton_Click;
            _locBackToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_lang.GetLangText(LangKey.GoBackMenu)).X / 2)), 500);

            _gameComponents = new List<Component>()
            {
                goMenuButton
            };
        }
        private void GoMenuButton_Click(object sender, EventArgs e)
        {
            GoToMainMenu();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameInstuction), _locTitle, Color.Black);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(50, 100), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorsOption), new Vector2(50, 130), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorRed), new Vector2(50, 160), Color.Red);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorBlue), new Vector2(50, 190), Color.Blue);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorGreen), new Vector2(50, 220), Color.Green);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.ControlsInGame), new Vector2(50, 280), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpLeftRight), new Vector2(50, 310), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpUpDown), new Vector2(50, 340), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpSpace), new Vector2(50, 370), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpEsc), new Vector2(50, 400), Color.Black);

            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.GoBackMenu), _locBackToolTip, Color.Black);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            foreach (var component in _gameComponents)
                component.Update();

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            _game1.GoToMainMenu(true);
        }
    }
}
