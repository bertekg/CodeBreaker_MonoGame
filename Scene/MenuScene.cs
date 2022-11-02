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
    internal class MenuScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _navigationScene, _optionFont, _creditsFont;
        private readonly Lang _lang;

        private readonly Texture2D _iconGame;
        private Vector2 _iconLocation;
        
        private readonly string _versionGame;
        
        private readonly bool _isDebugMode;

        private List<Component> _gameComponents;
        private Texture2D _buttonSprite;

        private bool _keyEscapeReleased;
        private Vector2 _locStartToolTip, _locModifiersToolTip, _locHelpToolTip, _locOptionToolTip, _locQuitToolTip;

        public MenuScene(Game1 game1, SpriteFont titleFont, SpriteFont navigationScene, SpriteFont optionFont,
            SpriteFont creditsFont, Lang lang, Texture2D iconGame, string versionGame, bool isDebugMode,
            Texture2D buttonSprite)
        {
            _game1 = game1;
            _titleFont = titleFont;
            _navigationScene = navigationScene;
            _optionFont = optionFont;
            _creditsFont = creditsFont;
            _lang = lang;
            _iconGame = iconGame;
            _iconLocation = new Vector2(60, 27);
            _versionGame = versionGame;
            
            _isDebugMode = isDebugMode;
            _buttonSprite = buttonSprite;
            InitializeButtons();
        }
        private void InitializeButtons()
        {
            float startX = (Game1.ScreenWidth / 2) - (_buttonSprite.Width / 2);
            Button startButton = new Button(_buttonSprite, _optionFont)
            {
                Position = new Vector2(startX, 130), Text = _lang.GetLangText(LangKey.Start)
            };
            startButton.Click += StartButton_Click;
            _locStartToolTip = new Vector2(((Game1.ScreenWidth / 2) - 
                (_optionFont.MeasureString(_lang.GetLangText(LangKey.StartGameKey)).X / 2)), 160);

            Button modifiersButton = new Button(_buttonSprite, _optionFont)
            {
                Position = new Vector2(startX, 210), Text = _lang.GetLangText(LangKey.Modifiers)
            };
            modifiersButton.Click += ModifiersButton_Click;
            _locModifiersToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_optionFont.MeasureString(_lang.GetLangText(LangKey.ModifiersKey)).X / 2)), 240);


            Button helpButton = new Button(_buttonSprite, _optionFont)
            {
                Position = new Vector2(startX, 290), Text = _lang.GetLangText(LangKey.Help)
            };
            helpButton.Click += HelpButton_Click;
            _locHelpToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_optionFont.MeasureString(_lang.GetLangText(LangKey.GameInstuctionKey)).X / 2)), 320);

            Button optionButton = new Button(_buttonSprite, _optionFont)
            {
                Position = new Vector2(startX, 370), Text = _lang.GetLangText(LangKey.Option)
            };
            optionButton.Click += OptiontButton_Click;
            _locOptionToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_optionFont.MeasureString(_lang.GetLangText(LangKey.GameOptionKey)).X / 2)), 400);

            Button quitButton = new Button(_buttonSprite, _optionFont)
            {
                Position = new Vector2(startX, 450), Text = _lang.GetLangText(LangKey.Quit)
            };
            quitButton.Click += QuitButton_Click;
            _locQuitToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_optionFont.MeasureString(_lang.GetLangText(LangKey.ExitGameKey)).X / 2)), 480);

            _gameComponents = new List<Component>()
            {
                startButton, modifiersButton, helpButton, optionButton, quitButton
            };
        }
        private void StartButton_Click(object sender, EventArgs e)
        {
            GoToGame();
        }
        private void ModifiersButton_Click(object sender, EventArgs e)
        {
            GoToModifiers();
        }
        private void HelpButton_Click(object sender, EventArgs e)
        {
            GoToInstuction();
        }
        private void OptiontButton_Click(object sender, EventArgs e)
        {
            GoToOption();
        }
        private void QuitButton_Click(object sender, EventArgs e)
        {
            ExitGame();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameTitle), new Vector2(180, 50), Color.Black);
            spriteBatch.Draw(_iconGame, _iconLocation, Color.White);

            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.StartGameKey), _locStartToolTip, Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.ModifiersKey), _locModifiersToolTip, Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameInstuctionKey), _locHelpToolTip, Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameOptionKey), _locOptionToolTip, Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.ExitGameKey), _locQuitToolTip, Color.Black);

            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.CreditsStart) + "Bartłomiej Grywalski", new Vector2(20, 530), Color.Black);
            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.VersionInfo) + _versionGame, new Vector2(600, 530), Color.Black);

            if (_isDebugMode)
            {
                spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.DebuggingModeEnabled), new Vector2(300, 10), Color.Black);
            }


            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                GoToGame();

            if (keyboardState.IsKeyDown(Keys.M) || gamePadState.Buttons.Y == ButtonState.Pressed)
                GoToModifiers();
            
            if (keyboardState.IsKeyDown(Keys.H) || gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                GoToInstuction();

            if (keyboardState.IsKeyDown(Keys.O) || gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                GoToOption();

            if ((keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                && _keyEscapeReleased == true)
            {
                ExitGame();
            }
            else if ((keyboardState.IsKeyUp(Keys.Escape) && gamePadState.Buttons.Back == ButtonState.Released)
                && _keyEscapeReleased == false)
                _keyEscapeReleased = true;


            foreach (var component in _gameComponents)
                component.Update();
        }
        private void GoToGame()
        {
            _game1.GoToGame();
        }
        private void GoToModifiers()
        {
            _game1.GoToModifiers();
        }
        private void GoToInstuction()
        {
            _game1.GoToInstuction();
        }
        private void GoToOption()
        {
            _game1.GoToOption();
        }
        private void ExitGame()
        {
            _game1.Exit();
        }
       
    }
}
