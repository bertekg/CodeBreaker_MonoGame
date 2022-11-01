using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Control;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame.Scene
{
    internal class ModifiersScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _instructionFont, _navigationFont;
        private readonly Lang _lang;
        private Marker _menuMarker;
        private bool _keyRightReleased, _keyLeftReleased, _keyUpReleased, _keyDownReleased;
        private int _menuMarkerIndex;

        private readonly SoundEffect _menuSideSoundEffect, _menuUpDownSoundEffect;
        private SaveData _saveData;
        const int MENU_MARKER_START_X = 225, MENU_MARKER_START_Y = 150, MENU_MARKER_STEP_Y = 40;

        private List<Component> _gameComponents;
        private readonly Texture2D _buttonNavigation, _buttonOption;
        public ModifiersScene(Game1 game1, SpriteFont titleFont, SpriteFont instructionFont, SpriteFont navigationFont, 
            Lang lang, Texture2D markerSprite, SaveData saveData, SoundEffect menuSideSoundEffect,
            SoundEffect menuUpDownSoundEffect, Texture2D buttonNavigation, Texture2D buttonOption)
        {
            _game1 = game1;
            _titleFont = titleFont;
            _instructionFont = instructionFont;
            _navigationFont = navigationFont;
            _lang = lang;
            _menuMarker = new Marker(markerSprite, 4, new Vector2(MENU_MARKER_START_X - 15, MENU_MARKER_START_Y - 5),
                new Vector2(210, MENU_MARKER_START_Y + (4 * MENU_MARKER_STEP_Y) - 5));
            _saveData = saveData;
            _menuSideSoundEffect = menuSideSoundEffect;
            _menuUpDownSoundEffect = menuUpDownSoundEffect;
            _buttonNavigation = buttonNavigation;
            _buttonOption = buttonOption;
            InitializeButtons();
            MoveMarker(0);
        }
        private void InitializeButtons()
        {
            Button goMenuButton = new Button(_buttonNavigation, _navigationFont)
            {
                Position = new Vector2(300, 465),
                Text = _lang.GetLangText(LangKey.Back)
            };
            goMenuButton.Click += GoMenuButton_Click;

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
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameModifiers), new Vector2(250, 50), Color.Black);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.CodeLength) + _saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.IsLimitedAttempts) + _lang.GetBoolInLang(_saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.NumberAttempts) + _saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), _saveData.isAttemptsLimit ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.IsTimeLimitation) + _lang.GetBoolInLang(_saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.TimeLimit) + _saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), _saveData.isTimeLimit ? Color.Black : Color.Gray);

            _menuMarker.Draw(spriteBatch);

            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.GoBackMenu), new Vector2(10, 500), Color.Black);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed)
               && _keyRightReleased == true)
            {
                switch (_menuMarkerIndex)
                {
                    case 0:
                        if (_saveData.codeLength < 5)
                        {
                            _saveData.codeLength++;
                        }
                        break;
                    case 1:
                        _saveData.isAttemptsLimit = true;
                        break;
                    case 2:
                        if (_saveData.attemptsLimit < 15)
                        {
                            _saveData.attemptsLimit++;
                        }
                        break;
                    case 3:
                        _saveData.isTimeLimit = true;
                        break;
                    case 4:
                        if (_saveData.timeLimit < 150)
                        {
                            _saveData.timeLimit += 10;
                        }
                        break;
                    default:
                        break;
                }
                _game1.PlaySoundEffect(_menuSideSoundEffect);
                _keyRightReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.D) && gamePadState.DPad.Right == ButtonState.Released)
               && _keyRightReleased == false)
            {
                _keyRightReleased = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
                && _keyLeftReleased == true)
            {
                switch (_menuMarkerIndex)
                {
                    case 0:
                        if (_saveData.codeLength > 3)
                        {
                            _saveData.codeLength--;
                        }
                        break;
                    case 1:
                        _saveData.isAttemptsLimit = false;
                        break;
                    case 2:
                        if (_saveData.attemptsLimit > 3)
                        {
                            _saveData.attemptsLimit--;
                        }
                        break;
                    case 3:
                        _saveData.isTimeLimit = false;
                        break;
                    case 4:
                        if (_saveData.timeLimit > 10)
                        {
                            _saveData.timeLimit -= 10;
                        }
                        break;
                    default:
                        break;
                }
                _game1.PlaySoundEffect(_menuSideSoundEffect);
                _keyLeftReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A) && gamePadState.DPad.Left == ButtonState.Released)
                && _keyLeftReleased == false)
            {
                _keyLeftReleased = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                && _keyUpReleased == true)
            {
                _menuMarkerIndex--;
                if (_menuMarkerIndex < 0)
                {
                    _menuMarkerIndex = 4;
                }
                if ((_menuMarkerIndex == 2 && _saveData.isAttemptsLimit == false) ||
                    (_menuMarkerIndex == 4 && _saveData.isTimeLimit == false))
                {
                    _menuMarkerIndex--;
                }
                MoveMarker(_menuMarkerIndex);
                _game1.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyUpReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W) && gamePadState.DPad.Up == ButtonState.Released)
                && _keyUpReleased == false)
            {
                _keyUpReleased = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
                && _keyDownReleased == true)
            {
                _menuMarkerIndex++;
                if ((_menuMarkerIndex == 2 && _saveData.isAttemptsLimit == false) ||
                    (_menuMarkerIndex == 4 && _saveData.isTimeLimit == false))
                {
                    _menuMarkerIndex++;
                }
                if (_menuMarkerIndex > 4)
                {
                    _menuMarkerIndex = 0;
                }
                MoveMarker(_menuMarkerIndex);
                _game1.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyDownReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
               && _keyDownReleased == false)
            {
                _keyDownReleased = true;
            }

            foreach (var component in _gameComponents)
                component.Update();

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                GoToMainMenu();
        }

        private void GoToMainMenu()
        {
            _game1.GoToMainMenu(true);
        }

        public void MoveMarker(int index)
        {
            _menuMarker.MoveMarker(index);
            _menuMarker.Update();
        }
        private Vector2 GetMenuMarkerPosition(int row)
        {
            return new Vector2(MENU_MARKER_START_X, MENU_MARKER_START_Y + (row * MENU_MARKER_STEP_Y));
        }
    }
}
