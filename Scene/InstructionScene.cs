using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CodeBreaker_MonoGame.Scene
{
    internal class InstructionScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _instructionFont;
        private readonly SpriteFont _navigationFont;
        private readonly Lang _lang;
        public InstructionScene(Game1 game1, SpriteFont titleFont, SpriteFont instructionFont, SpriteFont navigationFont, Lang lang)
        {
            _game1 = game1;
            _titleFont = titleFont;
            _instructionFont = instructionFont;
            _navigationFont = navigationFont;
            _lang = lang;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameInstuction), new Vector2(250, 50), Color.Black);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(50, 120), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorsOption), new Vector2(50, 150), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorRed), new Vector2(50, 180), Color.Red);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorBlue), new Vector2(50, 210), Color.Blue);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorGreen), new Vector2(50, 240), Color.Green);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.ControlsInGame), new Vector2(50, 300), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpLeftRight), new Vector2(50, 330), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpUpDown), new Vector2(50, 360), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpSpace), new Vector2(50, 390), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpEsc), new Vector2(50, 420), Color.Black);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 500), Color.Black);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                _game1.GoToMainMenu(true);
        }
    }
}
