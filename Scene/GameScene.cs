using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace CodeBreaker_MonoGame.Scene
{
    internal class GameScene : IScene
    {
        private readonly Game1 _game1;
        private readonly bool _isDebugMode;
        private readonly SaveData _saveData;
        private readonly SpriteFont _digitFont;
        private readonly SpriteFont _instructionFont;
        private readonly SpriteFont _modFont;
        private GameLogic _gameLogic;
        private readonly Texture2D _markerSprite;
        private readonly Lang _lang;
        private readonly Texture2D _attemptIconReady, _attemptIconUsed;
        private readonly Texture2D _squereBaseSprite;
        private readonly SoundEffect _sideSoundEffect, _upDownSoundEffect, _unlockTrySoundEffect;
        private readonly SoundEffect _successSoundEffect, _failSoundEffect;

        private int _markerIndex;
        private float _markerPosition;

        private int _attemptIconStartX, _attemptIconStartY, _attemptIconStepX, _attmptIconSize;
        public double _playingTime, _remainingTime;
        private Rectangle _timeLimitBarBase;

        private Vector2 _guessCodeHistoryTextPlace;

        private Color _tableColor = Color.Black;

        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private bool _keySpaceUp;

        const int CODE_POSITION_START_X = 50, CODE_POSITION_START_Y = 180, CODE_POSITION_STEP_X = 105;
        const int CODE_OFFSET_MARKER_X = -20, CODE_OFFSET_MARKER_Y = -2;

        const int GUESS_HISTORY_START_X = 600, GUESS_HISTORY_START_Y = 115;
        const int GUESS_HISTORY_STEP_X = 40, GUESS_HISTORY_STEP_Y = 50, GUESS_HISTORY_THICKNESS = 3;
        const int GUESS_TABLE_OFFSET_X = -11, gUESS_TABLE_OFFSET_Y = -3;

        const int TIME_LIMIT_BAR_START_X = 10, TIME_LIMIT_BAR_START_Y = 120, TIME_LIMIT_BAR_HEIGHT = 20, TIME_LIMIT_BAR_WIDTH_MAX = 320;

        public GameScene(Game1 game1, bool isDebugMode, SaveData saveData, SpriteFont digitFont, SpriteFont instructionFont,
            SpriteFont modFont, GameLogic gameLogic, Texture2D markerSprite, Lang lang, Texture2D attemptIconReady,
            Texture2D attemptIconUsed, Texture2D squereBaseSprite, SoundEffect sideSoundEffect, SoundEffect upDownSoundEffect,
            SoundEffect unlockTrySoundEffect, SoundEffect successSoundEffect, SoundEffect failSoundEffect)
        {
            _game1 = game1;
            _isDebugMode = isDebugMode;
            _saveData = saveData;
            _digitFont = digitFont;
            _instructionFont = instructionFont;
            _modFont = modFont;
            _gameLogic = gameLogic;
            _markerSprite = markerSprite;
            _lang = lang;
            _attemptIconReady = attemptIconReady;
            _attemptIconUsed = attemptIconUsed;
            _squereBaseSprite = squereBaseSprite;
            _sideSoundEffect = sideSoundEffect;
            _upDownSoundEffect = upDownSoundEffect;
            _unlockTrySoundEffect = unlockTrySoundEffect;
            _successSoundEffect = successSoundEffect;
            _failSoundEffect = failSoundEffect;

            if (_saveData.isAttemptsLimit)
            {
                if (_saveData.attemptsLimit < 10)
                {
                    _attmptIconSize = 42;
                    _attemptIconStartX = 395;
                    _attemptIconStepX = 46;
                    _attemptIconStartY = 65;
                }
                else
                {
                    _attmptIconSize = 24;
                    _attemptIconStartX = 415;
                    _attemptIconStepX = 26;
                    _attemptIconStartY = 70;
                }
            }

            _markerIndex = 0;
            _playingTime = 0;
            _remainingTime = _saveData.timeLimit;

            _timeLimitBarBase = new Rectangle(TIME_LIMIT_BAR_START_X, TIME_LIMIT_BAR_START_Y, TIME_LIMIT_BAR_WIDTH_MAX, TIME_LIMIT_BAR_HEIGHT);

            _markerIndex = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _saveData.codeLength; i++)
            {
                spriteBatch.DrawString(_digitFont, _gameLogic.currentCode[i].ToString(), new Vector2(CODE_POSITION_START_X + (CODE_POSITION_STEP_X * i), CODE_POSITION_START_Y), Color.Black);
                spriteBatch.Draw(_markerSprite, new Vector2(CODE_POSITION_START_X + CODE_OFFSET_MARKER_X + (i * CODE_POSITION_STEP_X), CODE_POSITION_START_Y + CODE_OFFSET_MARKER_Y), i == _markerIndex ? Color.Purple : Color.Gray);
            }

            if (!_isDebugMode)
            {
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(5, 400), Color.Black);
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorsOption), new Vector2(5, 430), Color.Black);
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorRed), new Vector2(5, 460), Color.Red);
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorBlue), new Vector2(5, 490), Color.Blue);
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.HelpColorGreen), new Vector2(5, 520), Color.Green);
            }
            else
            {
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.DebugMarkerIndex) + _markerIndex.ToString()
                    + _lang.GetLangText(LangKey.DebugMarkerPos) + _markerPosition.ToString(), new Vector2(10, 400), Color.Black);
                spriteBatch.DrawString(_instructionFont, _lang.GetLangText(LangKey.DebugCurrentCode) + _gameLogic.CurrentCodeString(), new Vector2(10, 430), Color.Black);
            }

            if (_saveData.isAttemptsLimit)
            {
                int remainingAttempt = _saveData.attemptsLimit - _gameLogic.numberOfAttempts;
                spriteBatch.DrawString(_modFont, _lang.GetLangText(LangKey.GameRemainingAttempts) + remainingAttempt.ToString(), new Vector2(360, 10), Color.Black);
                for (int i = 0; i < _saveData.attemptsLimit; i++)
                {
                    if (i < remainingAttempt)
                    {
                        spriteBatch.Draw(_attemptIconReady, new Rectangle(_attemptIconStartX + (i * _attemptIconStepX), _attemptIconStartY, _attmptIconSize, _attmptIconSize), Color.White);
                    }
                    else
                    {
                        spriteBatch.Draw(_attemptIconUsed, new Rectangle(_attemptIconStartX + (i * _attemptIconStepX), _attemptIconStartY, _attmptIconSize, _attmptIconSize), Color.White);
                    }
                }
            }
            else
            {
                spriteBatch.DrawString(_modFont, _lang.GetLangText(LangKey.GameNumberOfAttempts) + _gameLogic.numberOfAttempts.ToString(), new Vector2(360, 10), Color.Black);
            }

            if (_saveData.isTimeLimit)
            {
                spriteBatch.DrawString(_modFont, string.Format(_lang.GetLangText(LangKey.GameRemainingTime), _remainingTime), new Vector2(10, 10), Color.Black);
                spriteBatch.Draw(_squereBaseSprite, _timeLimitBarBase, Color.White);
                spriteBatch.Draw(_squereBaseSprite, GetTimieLimitBarRectangel(), GetTimeLimitBarColor());
            }

            for (int i = 0; i < _gameLogic.guessCodeHistory.Count; i++)
            {
                for (int j = 0; j < _gameLogic.guessCodeHistory[i].Count; j++)
                {
                    _guessCodeHistoryTextPlace = new Vector2(GUESS_HISTORY_START_X + (j * GUESS_HISTORY_STEP_X), GUESS_HISTORY_START_Y + i * GUESS_HISTORY_STEP_Y);
                    spriteBatch.DrawString(_modFont, _gameLogic.guessCodeHistory[i][j].value.ToString(), _guessCodeHistoryTextPlace, DecodeColor(_gameLogic.guessCodeHistory[i][j].digitState));
                }
            }

            if (_gameLogic.guessCodeHistory.Count > 0)
            {
                for (int i = 0; i < _gameLogic.guessCodeHistory.Count; i++)
                {
                    spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X, (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y) + i * GUESS_HISTORY_STEP_Y,
                                        GUESS_HISTORY_STEP_X * _gameLogic.guessCodeHistory[0].Count, GUESS_HISTORY_THICKNESS), _tableColor);
                    if (i == _gameLogic.guessCodeHistory.Count - 1)
                    {
                        spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X, (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y) + (i + 1) * GUESS_HISTORY_STEP_Y,
                                            GUESS_HISTORY_STEP_X * _gameLogic.guessCodeHistory[0].Count, GUESS_HISTORY_THICKNESS), _tableColor);
                    }
                }
                for (int i = 0; i < _gameLogic.guessCodeHistory[0].Count; i++)
                {
                    spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X + (i * GUESS_HISTORY_STEP_X), (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y),
                                        GUESS_HISTORY_THICKNESS, GUESS_HISTORY_STEP_Y * _gameLogic.guessCodeHistory.Count), _tableColor);
                    if (i == _gameLogic.guessCodeHistory[0].Count - 1)
                    {
                        spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X + ((i + 1) * GUESS_HISTORY_STEP_X), (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y),
                                        GUESS_HISTORY_THICKNESS, (GUESS_HISTORY_STEP_Y * _gameLogic.guessCodeHistory.Count) + GUESS_HISTORY_THICKNESS), _tableColor);
                    }
                }
            }
        }

        public void Update(double deltaTime)
        {
            _markerPosition = 30.0f + (_markerIndex * 100.0f);

            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                _game1.GoToMainMenu(true);

            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed)
                && _keyRightUp == true)
            {
                _markerIndex++;
                if (_markerIndex > _saveData.codeLength - 1)
                {
                    _markerIndex = 0;
                }
                _game1.PlaySoundEffect(_sideSoundEffect);
                _keyRightUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.D) && gamePadState.DPad.Right == ButtonState.Released)
                && _keyRightUp == false)
            {
                _keyRightUp = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
               && _keyLeftUp == true)
            {
                _markerIndex--;
                if (_markerIndex < 0)
                {
                    _markerIndex = _saveData.codeLength - 1;
                }
                _game1.PlaySoundEffect(_sideSoundEffect);
                _keyLeftUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A) && gamePadState.DPad.Left == ButtonState.Released)
                && _keyLeftUp == false)
            {
                _keyLeftUp = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                && _keyUpUp == true)
            {
                _gameLogic.currentCode[_markerIndex]++;
                if (_gameLogic.currentCode[_markerIndex] > 9)
                {
                    _gameLogic.currentCode[_markerIndex] = 0;
                }
                _game1.PlaySoundEffect(_upDownSoundEffect);
                _keyUpUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W) && gamePadState.DPad.Up == ButtonState.Released)
                && _keyUpUp == false)
            {
                _keyUpUp = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
              && _keyDownUp == true)
            {
                _gameLogic.currentCode[_markerIndex]--;
                if (_gameLogic.currentCode[_markerIndex] < 0)
                {
                    _gameLogic.currentCode[_markerIndex] = 9;
                }
                _game1.PlaySoundEffect(_upDownSoundEffect);
                _keyDownUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
                && _keyDownUp == false)
            {
                _keyDownUp = true;
            }

            if ((keyboardState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
                && _keySpaceUp == true)
            {
                bool isCodeCorrect = _gameLogic.TryCode();
                if (isCodeCorrect)
                {
                    _game1.GoToFinish(_successSoundEffect);
                }
                else if (((_gameLogic.numberOfAttempts >= _saveData.attemptsLimit) && _saveData.isAttemptsLimit))
                {
                    _game1.GoToFinish(_failSoundEffect);
                }
                else
                {
                    _game1.PlaySoundEffect(_unlockTrySoundEffect);
                }
                _keySpaceUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Space) && gamePadState.Buttons.A == ButtonState.Released)
                && _keySpaceUp == false)
            {
                _keySpaceUp = true;
            }

            if (_saveData.isTimeLimit)
            {
                _remainingTime -= deltaTime;
                if (_remainingTime <= 0)
                {
                    _remainingTime = 0;
                    _game1.GoToFinish(_failSoundEffect);
                }
            }
            else
            {
                _playingTime += deltaTime;
            }
        }

        private Rectangle GetTimieLimitBarRectangel()
        {
            float barWidth = ((float)_remainingTime / (float)_saveData.timeLimit) * TIME_LIMIT_BAR_WIDTH_MAX;
            return new Rectangle(TIME_LIMIT_BAR_START_X, TIME_LIMIT_BAR_START_Y, (int)barWidth, TIME_LIMIT_BAR_HEIGHT);
        }

        private Color GetTimeLimitBarColor()
        {
            float remainingTimeFraction = (float)_remainingTime / (float)_saveData.timeLimit;
            Color barColor;
            if (remainingTimeFraction >= 0.8f)
            {
                barColor = new Color(44, 186, 0);
            }
            else if (remainingTimeFraction >= 0.6f && remainingTimeFraction < 0.8f)
            {
                barColor = new Color(163, 255, 0);
            }
            else if (remainingTimeFraction >= 0.4f && remainingTimeFraction < 0.6f)
            {
                barColor = new Color(255, 244, 0);
            }
            else if (remainingTimeFraction >= 0.2f && remainingTimeFraction < 0.4f)
            {
                barColor = new Color(255, 167, 0);
            }
            else
            {
                barColor = new Color(255, 0, 0);
            }
            return barColor;
        }
        private Color DecodeColor(DigitState digitState)
        {
            Color colorDecoded = new Color();
            switch (digitState)
            {
                case DigitState.Good:
                    colorDecoded = Color.Green;
                    break;
                case DigitState.Bad:
                    colorDecoded = Color.Red;
                    break;
                case DigitState.Diffrent:
                    colorDecoded = Color.Blue;
                    break;
                default:
                    break;
            }
            return colorDecoded;
        }
    }
}
