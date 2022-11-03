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
        const int MENU_MARKER_START_X = 225, MENU_MARKER_START_Y = 100, MENU_MARKER_STEP_Y = 40;

        private List<Component> _gameComponents;
        private readonly Texture2D _buttonNavigation, _buttonOption;

        private Vector2 _locTitle, _locBackToolTip, _locStartToolTip;
        private readonly Vector2 _offsetButtonOptionRight, _offsetButtonOptionLeft;
        Button _numberAttemptsIncButton, _numberAttemptsDecButton;
        Button _timeLimitationIncButton, _timeLimitationDecButton; 
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
            _offsetButtonOptionRight = new Vector2(-55, 0);
            _offsetButtonOptionLeft = new Vector2(-90, 0);
            InitializeButtons();
            MoveMarker(0);
            _locTitle = new Vector2((Game1.ScreenWidth / 2) - 
                (_titleFont.MeasureString(_lang.GetLangText(LangKey.GameModifiers)).X / 2), 30);
        }
        private void InitializeButtons()
        {
            float startX = (Game1.ScreenWidth / 2) - (_buttonNavigation.Width / 2);

            Button goMenuButton = new Button(_buttonNavigation, _navigationFont)
            {
                Position = new Vector2(startX, 470),
                Text = _lang.GetLangText(LangKey.Back)
            };
            goMenuButton.Click += GoMenuButton_Click;
            _locBackToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_lang.GetLangText(LangKey.GoBackMenu)).X / 2)), 500);

            Button startGameButton = new Button(_buttonNavigation, _navigationFont)
            {
                Position = new Vector2(startX, 390),
                Text = _lang.GetLangText(LangKey.Start)
            };
            startGameButton.Click += StartGameButton_Click;
            _locStartToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_lang.GetLangText(LangKey.StartGameKey)).X / 2)), 420);

            Button codeLenIncButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(0) + _offsetButtonOptionRight,
                Text = "+"
            };
            codeLenIncButton.Click += CodeLenInc_Click;

            Button codeLenDecButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(0) + _offsetButtonOptionLeft,
                Text = "-"
            };
            codeLenDecButton.Click += CodeLenDec_Click;

            Button isLimitedAttemptsTrueButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(1) + _offsetButtonOptionRight,
                Text = _lang.GetLangText(LangKey.TrueSingleLetter)
            };
            isLimitedAttemptsTrueButton.Click += IsLimitedAttemptsTrue_Click;

            Button isLimitedAttemptsFalseButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(1) + _offsetButtonOptionLeft,
                Text = _lang.GetLangText(LangKey.FalseSingleLetter)
            };
            isLimitedAttemptsFalseButton.Click += IsLimitedAttemptsFalse_Click;

            _numberAttemptsIncButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(2) + _offsetButtonOptionRight,
                Text = "+",
                IsDisable = !_saveData.isAttemptsLimit
            };
            _numberAttemptsIncButton.Click += NumberAttemptsInc_Click;

            _numberAttemptsDecButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(2) + _offsetButtonOptionLeft,
                Text = "-",
                IsDisable = !_saveData.isAttemptsLimit
            };
            _numberAttemptsDecButton.Click += NumberAttemptsDec_Click;

            Button isTimeLimitationTrueButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(3) + _offsetButtonOptionRight,
                Text = _lang.GetLangText(LangKey.TrueSingleLetter)
            };
            isTimeLimitationTrueButton.Click += IsTimeLimitationTrue_Click;

            Button isTimeLimitationFalseButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(3) + _offsetButtonOptionLeft,
                Text = _lang.GetLangText(LangKey.FalseSingleLetter)
            };
            isTimeLimitationFalseButton.Click += IsTimeLimitationFalse_Click;

            _timeLimitationIncButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(4) + _offsetButtonOptionRight,
                Text = "+",
                IsDisable = !_saveData.isTimeLimit
            };
            _timeLimitationIncButton.Click += TimeLimitationInc_Click;

            _timeLimitationDecButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(4) + _offsetButtonOptionLeft,
                Text = "-",
                IsDisable = !_saveData.isTimeLimit
            };
            _timeLimitationDecButton.Click += TimeLimitationDec_Click;

            Button digitRangIncButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(5) + _offsetButtonOptionRight,
                Text = "+",
            };
            digitRangIncButton.Click += DigitRangInc_Click;

            Button digitRangDecButton = new Button(_buttonOption, _instructionFont)
            {
                Position = GetMenuMarkerPosition(5) + _offsetButtonOptionLeft,
                Text = "-",
            };
            digitRangDecButton.Click += DigitRangDec_Click;

            _gameComponents = new List<Component>()
            {
                goMenuButton, startGameButton, codeLenIncButton, codeLenDecButton, isLimitedAttemptsTrueButton,
                isLimitedAttemptsFalseButton, isTimeLimitationTrueButton, isTimeLimitationFalseButton,
                _numberAttemptsIncButton, _numberAttemptsDecButton, _timeLimitationIncButton, _timeLimitationDecButton,
                digitRangIncButton, digitRangDecButton
            };
        }
        private void DigitRangInc_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 5;
            MoveMarker(_menuMarkerIndex);
            MaxDigitInc();
        }
        private void DigitRangDec_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 5;
            MoveMarker(_menuMarkerIndex);
            MaxDigitDec();
        }
        private void GoMenuButton_Click(object sender, EventArgs e)
        {
            GoToMainMenu();
        }
        private void StartGameButton_Click(object sender, EventArgs e)
        {
            GoToGame();            
        }

        private void GoToGame()
        {
            _game1.GoToGame();
        }

        private void CodeLenInc_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 0;
            MoveMarker(_menuMarkerIndex);
            CodeLengthInc();
        }
        private void CodeLenDec_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 0;
            MoveMarker(_menuMarkerIndex);
            CodeLengthDec();
        }
        private void IsLimitedAttemptsTrue_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 1;
            MoveMarker(_menuMarkerIndex);
            IsLimitedAttemptsTrue();
        }
        private void IsLimitedAttemptsFalse_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 1;
            MoveMarker(_menuMarkerIndex);
            IsLimitedAttemptsFalse();
        }
        private void NumberAttemptsInc_Click(object sender, EventArgs e)
        {
            if (_saveData.isAttemptsLimit == false) return;
            _menuMarkerIndex = 2;
            MoveMarker(_menuMarkerIndex);
            NumberAttemptsInc();
        }
        private void NumberAttemptsDec_Click(object sender, EventArgs e)
        {
            if (_saveData.isAttemptsLimit == false) return;
            _menuMarkerIndex = 2;
            MoveMarker(_menuMarkerIndex);
            NumberAttemptsDec();
        }
        private void IsTimeLimitationTrue_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 3;
            MoveMarker(_menuMarkerIndex);
            IsTimeLimitationTrue();
        }
        private void IsTimeLimitationFalse_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 3;
            MoveMarker(_menuMarkerIndex);
            IsTimeLimitationFalse();
        }
        private void TimeLimitationInc_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 4;
            MoveMarker(_menuMarkerIndex);
            TimeLimitationInc();
        }
        private void TimeLimitationDec_Click(object sender, EventArgs e)
        {
            _menuMarkerIndex = 4;
            MoveMarker(_menuMarkerIndex);
            TimeLimitationDec();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameModifiers), _locTitle, Color.Black);

            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.CodeLength) + _saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.IsLimitedAttempts) + _lang.GetBoolInLang(_saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.NumberAttempts) + _saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), _saveData.isAttemptsLimit ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.IsTimeLimitation) + _lang.GetBoolInLang(_saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.Black);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.TimeLimit) + _saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), _saveData.isTimeLimit ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.DigitRange) + ": 0 - " + _saveData.maxCodeDigit.ToString("X"), GetMenuMarkerPosition(5), Color.Black);
            spriteBatch.DrawString(_instructionFont, "TODO History length", GetMenuMarkerPosition(6), Color.Black);

            _menuMarker.Draw(spriteBatch);

            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.StartGameKey), _locStartToolTip, Color.Black);
            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.GoBackMenu), _locBackToolTip, Color.Black);
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
                        CodeLengthInc();
                        break;
                    case 1:
                        IsLimitedAttemptsTrue();
                        break;
                    case 2:
                        NumberAttemptsInc();
                        break;
                    case 3:
                        IsTimeLimitationTrue();
                        break;
                    case 4:
                        TimeLimitationInc();
                        break;
                    case 5:
                        MaxDigitInc();
                        break;
                    default:
                        break;
                }
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
                        CodeLengthDec();
                        break;
                    case 1:
                        IsLimitedAttemptsFalse();
                        break;
                    case 2:
                        NumberAttemptsDec();
                        break;
                    case 3:
                        IsTimeLimitationFalse();
                        break;
                    case 4:
                        TimeLimitationDec();
                        break;
                    case 5:
                        MaxDigitDec();
                        break;
                    default:
                        break;
                }
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
                    _menuMarkerIndex = 5;
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
                if (_menuMarkerIndex > 5)
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

            if (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                GoToGame();

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                GoToMainMenu();
        }
        private void MaxDigitDec()
        {
            _saveData.maxCodeDigit -= 2;
            if (_saveData.maxCodeDigit < 5)
            {
                _saveData.maxCodeDigit = 5;
            }
            PlaySideSound();
        }
        private void MaxDigitInc()
        {
            _saveData.maxCodeDigit += 2;
            if (_saveData.maxCodeDigit > 15)
            {
                _saveData.maxCodeDigit = 15;
            }
            PlaySideSound();
        }

        private void TimeLimitationDec()
        {
            if (_saveData.timeLimit > 10)
            {
                _saveData.timeLimit -= 10;
            }
            PlaySideSound();
        }

        private void TimeLimitationInc()
        {
            if (_saveData.timeLimit < 150)
            {
                _saveData.timeLimit += 10;
            }
            PlaySideSound();
        }

        private void NumberAttemptsDec()
        {
            if (_saveData.attemptsLimit > 3)
            {
                _saveData.attemptsLimit--;
            }
            PlaySideSound();
        }

        private void NumberAttemptsInc()
        {
            if (_saveData.attemptsLimit < 15)
            {
                _saveData.attemptsLimit++;
            }
            PlaySideSound();
        }

        private void IsTimeLimitationFalse()
        {
            _saveData.isTimeLimit = false;
            _timeLimitationIncButton.IsDisable = true;
            _timeLimitationDecButton.IsDisable = true;
            PlaySideSound();
        }

        private void IsTimeLimitationTrue()
        {
            _saveData.isTimeLimit = true;
            _timeLimitationIncButton.IsDisable = false;
            _timeLimitationDecButton.IsDisable = false;
            PlaySideSound();
        }

        private void IsLimitedAttemptsFalse()
        {
            _saveData.isAttemptsLimit = false;
            _numberAttemptsIncButton.IsDisable = true;
            _numberAttemptsDecButton.IsDisable = true;
            PlaySideSound();
        }

        private void IsLimitedAttemptsTrue()
        {
            _saveData.isAttemptsLimit = true;
            _numberAttemptsIncButton.IsDisable = false;
            _numberAttemptsDecButton.IsDisable = false;
            PlaySideSound();
        }

        private void PlaySideSound()
        {
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void CodeLengthDec()
        {
            if (_saveData.codeLength > 3)
            {
                _saveData.codeLength--;
            }
            PlaySideSound();
        }

        private void CodeLengthInc()
        {
            if (_saveData.codeLength < 5)
            {
                _saveData.codeLength++;
            }
            PlaySideSound();
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
