using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CodeBreaker_MonoGame.Scene
{
    internal class MenuScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _navigationScene, _optionFont, _creditsFont;
        private readonly Lang _lang;

        private readonly Texture2D _iconGame;
        private Vector2 _iconLocation;

        const int MENU_MARKER_START_X = 225, MENU_MARKER_START_Y = 330, MENU_MARKER_STEP_Y = 40;

        private SaveData _saveData;
        private readonly string _versionGame;

        private Marker _menuMarker;
        private bool _keyEscapeReleased, _keyRightReleased, _keyLeftReleased, _keyUpReleased, _keyDownReleased;
        private int _menuMarkerIndex;

        private readonly SoundEffect _menuSideSoundEffect, _menuUpDownSoundEffect;
        private readonly bool _isDebugMode;
        public MenuScene(Game1 game1, SpriteFont titleFont, SpriteFont navigationScene, SpriteFont optionFont,
            SpriteFont creditsFont, Lang lang, Texture2D iconGame, string versionGame, Texture2D markerSprite,
            SaveData saveData, SoundEffect menuSideSoundEffect, SoundEffect menuUpDownSoundEffect, bool isDebugMode)
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
            _menuMarker = new Marker(markerSprite, 4, new Vector2(MENU_MARKER_START_X - 15, MENU_MARKER_START_Y - 5),
                new Vector2(210, MENU_MARKER_START_Y + (4 * MENU_MARKER_STEP_Y) - 5));
            _saveData = saveData;
            _menuSideSoundEffect = menuSideSoundEffect;
            _menuUpDownSoundEffect = menuUpDownSoundEffect;
            _isDebugMode = isDebugMode;
            MoveMarker(0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameTitle), new Vector2(180, 50), Color.Black);
            spriteBatch.Draw(_iconGame, _iconLocation, Color.White);

            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.StartGameKey), new Vector2(120, 140), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameInstuctionKey), new Vector2(120, 170), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameOptionKey), new Vector2(120, 200), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.ExitGameKey), new Vector2(120, 230), Color.Black);

            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.GameModifiers), GetMenuMarkerPosition(-1), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.CodeLength) + _saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.IsLimitedAttempts) + _lang.GetBoolInLang(_saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.NumberAttempts) + _saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), _saveData.isAttemptsLimit ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.IsTimeLimitation) + _lang.GetBoolInLang(_saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.TimeLimit) + _saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), _saveData.isTimeLimit ? Color.Black : Color.Gray);

            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.CreditsStart) + "Bartłomiej Grywalski", new Vector2(20, 530), Color.Black);
            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.VersionInfo) + _versionGame, new Vector2(600, 530), Color.Black);

            if (_isDebugMode)
            {
                spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.DebuggingModeEnabled), new Vector2(300, 10), Color.Black);
            }

            _menuMarker.Draw(spriteBatch);
        }
        public void Update(double deltaTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                _game1.GoToGame();

            if (keyboardState.IsKeyDown(Keys.H) || gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
            {
                _game1.GoToInstuction();
            }

            if (keyboardState.IsKeyDown(Keys.O) || gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
            {
                _game1.GoToOption();
            }

            if ((keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                && _keyEscapeReleased == true)
            {
                _game1.Exit();
            }
            else if ((keyboardState.IsKeyUp(Keys.Escape) && gamePadState.Buttons.Back == ButtonState.Released)
                && _keyEscapeReleased == false)
                _keyEscapeReleased = true;

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
