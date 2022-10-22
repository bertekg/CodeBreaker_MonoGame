using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace CodeBreaker_MonoGame
{
    public enum GameState { Menu, GameInstructions, Option,  InGame, FinishGame}
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _largeFont, _bigFont, _middleFont, _smallFont, _littleFont;

        private Texture2D _menuMarkerSprite, _gameMarkerSprite;

        private MusicAndSounds _musicAndSounds;

        private SoundEffect _menuUpDownSoundEffect, _menuSideSoundEffect;
        private SoundEffect _startSoundEffect, _returnMenuSoundEffect;
        private SoundEffect _gameUpDownSoundEffect, _gameSideSoundEffect, _unlockTrySoundEffect;
        private SoundEffect _successSoundEffect, _failSoundEffect;
        private SoundEffect _menuNaviInstr, _menuNaviOption;

        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private bool _keyEnterUp, _keyHelpUp, _keyOptionUp, _keyEscapeUp, _keySpaceUp;

        GameLogic gameLogic;

        const bool IS_DEBUG_MODE = false;

        GameState gameState;

        private int _menuMarkerIndex;
        private float _menuMarkerPosition;

        private double _playingTime, _remainingTime;

        private int _gameMarkerIndex;
        private float _gameMarkerPosition;

        const int MAX_NUMBER_OF_HINTS = 8;

        private Vector2 _guessCodeHistoryTextPlace;

        private string _endGameInfo;
        private Color _endGameColor;

        string _saveDataPath;

        private Lang lang;

        private string versionText = "1.3.1 (2022.10.22)";

        private int _menuMarkerStartX = 225, _menuMarkerStartY = 200, _menuMarkerStepY = 40;

        private int _optionMarkerIndex;
        private float _optionMarkerPosition;

        private int _optionMarkerStartX = 225, _optionMarkerStartY = 150, _optionMarkerStepY = 40;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.Title = "Code Breaker - MonoGame";
            InitializePrivatVariables();
            GoToMainMenu();
            ReadSaveData();
            _graphics.PreferredBackBufferWidth = 820;
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();
            lang = new Lang(saveData.langID);

            base.Initialize();
        }
        private void InitializePrivatVariables()
        {
            _saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");

            _keyRightUp = true; _keyLeftUp = true; _keyUpUp = true; _keyDownUp = true;
            _keyEnterUp = true; _keyHelpUp = true; _keyOptionUp = true; _keyEscapeUp = true; _keySpaceUp = true;
    }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _largeFont = Content.Load<SpriteFont>("fonts/large");
            _bigFont = Content.Load<SpriteFont>("fonts/big");
            _middleFont = Content.Load<SpriteFont>("fonts/middle");
            _smallFont = Content.Load<SpriteFont>("fonts/small");
            _littleFont = Content.Load<SpriteFont>("fonts/little");

            _menuMarkerSprite = Content.Load<Texture2D>("sprites/menuMarker");
            _gameMarkerSprite = Content.Load<Texture2D>("sprites/gameMarker");

            Song song = Content.Load<Song>("audio/music");
            _musicAndSounds = new MusicAndSounds(song, saveData);
            _musicAndSounds.Play();

            _menuUpDownSoundEffect = Content.Load<SoundEffect>("sounds/menuUpDown");
            _menuSideSoundEffect = Content.Load<SoundEffect>("sounds/menuSide");
            _startSoundEffect = Content.Load<SoundEffect>("sounds/start");
            _returnMenuSoundEffect = Content.Load<SoundEffect>("sounds/return");
            _gameUpDownSoundEffect = Content.Load<SoundEffect>("sounds/gameUpDown");
            _gameSideSoundEffect = Content.Load<SoundEffect>("sounds/gameSide");
            _unlockTrySoundEffect = Content.Load<SoundEffect>("sounds/unlockTry");
            _successSoundEffect = Content.Load<SoundEffect>("sounds/success");
            _failSoundEffect = Content.Load<SoundEffect>("sounds/fail");
            _menuNaviInstr = Content.Load<SoundEffect>("sounds/menuNaviInst");
            _menuNaviOption = Content.Load<SoundEffect>("sounds/menuNaviOption");

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if ( (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
                && _keyEnterUp == true)
            {
                if (gameState == GameState.Menu || gameState == GameState.FinishGame)
                {
                    _musicAndSounds.PlaySoundEffect(_startSoundEffect);
                    StartNewGame();
                }
                _keyEnterUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Enter) && gamePadState.Buttons.Start == ButtonState.Released)
                && _keyEnterUp == false)
            {
                _keyEnterUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.H) || gamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                && _keyHelpUp == true)
            {
                if (gameState == GameState.Menu)
                {
                    _musicAndSounds.PlaySoundEffect(_menuNaviInstr);
                    gameState = GameState.GameInstructions;
                }   
                _keyHelpUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.H) && gamePadState.Buttons.RightShoulder == ButtonState.Released)
                && _keyHelpUp == false)
            {
                _keyHelpUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.O) || gamePadState.Buttons.LeftShoulder == ButtonState.Pressed)
                && _keyOptionUp == true)
            {
                if (gameState == GameState.Menu)
                {
                    _musicAndSounds.PlaySoundEffect(_menuNaviOption);
                    gameState = GameState.Option;
                    _optionMarkerIndex = 0;
                }
                _keyOptionUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.O) && gamePadState.Buttons.LeftShoulder == ButtonState.Released)
                && _keyOptionUp == false)
            {
                _keyOptionUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                && _keyEscapeUp == true)
            {
                if (gameState == GameState.Menu)
                {
                    Exit();
                }
                else
                {
                    _musicAndSounds.PlaySoundEffect(_returnMenuSoundEffect);
                    GoToMainMenu();
                }
                _keyEscapeUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Escape) && gamePadState.Buttons.Back == ButtonState.Released)
                && _keyEscapeUp == false)
            {
                _keyEscapeUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed)
                && _keyRightUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    _gameMarkerIndex++;
                    if (_gameMarkerIndex > saveData.codeLength - 1)
                    {
                        _gameMarkerIndex = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
                }
                else if (gameState == GameState.Menu)
                {
                    switch (_menuMarkerIndex)
                    {
                        case 0:
                            if (saveData.codeLength < 5)
                            {
                                saveData.codeLength++;
                            }
                            break;
                        case 1:
                            saveData.isAttemptsLimit = true;
                            break;
                        case 2:
                            if (saveData.attemptsLimit < 15)
                            {
                                saveData.attemptsLimit++;
                            }
                            break;
                        case 3:
                            saveData.isTimeLimit = true;
                            break;
                        case 4:
                            if (saveData.timeLimit < 150)
                            {
                                saveData.timeLimit += 10;
                            }
                            break;
                        default:
                            break;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);
                }
                else if(gameState == GameState.Option)
                {
                    switch (_optionMarkerIndex)
                    {
                        case 0:
                            _musicAndSounds.EditIsSounding(true);
                            break;
                        case 1:
                            _musicAndSounds.IncMusicVolume();
                            break;
                        case 2:
                            _musicAndSounds.IncSoundsVolume();
                            break;
                        case 3:
                            lang.IncSelection();
                            break;
                        default:
                            break;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);

                }
                _keyRightUp = false;
            }
            else if( (keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.D) && gamePadState.DPad.Right == ButtonState.Released)
                && _keyRightUp == false)
            {
                _keyRightUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
                && _keyLeftUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    _gameMarkerIndex--;
                    if (_gameMarkerIndex < 0)
                    {
                        _gameMarkerIndex = saveData.codeLength - 1;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
                }
                else if (gameState == GameState.Menu)
                {
                    switch (_menuMarkerIndex)
                    {
                        case 0:
                            if (saveData.codeLength > 3)
                            {
                                saveData.codeLength--;
                            }
                            break;
                        case 1:
                            saveData.isAttemptsLimit = false;
                            break;
                        case 2:
                            if (saveData.attemptsLimit > 3)
                            {
                                saveData.attemptsLimit--;
                            }
                            break;
                        case 3:
                            saveData.isTimeLimit = false;
                            break;
                        case 4:
                            if (saveData.timeLimit > 10)
                            {
                                saveData.timeLimit -= 10;
                            }
                            break;
                        default:
                            break;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);
                }
                else if (gameState == GameState.Option)
                {
                    switch (_optionMarkerIndex)
                    {
                        case 0:
                            _musicAndSounds.EditIsSounding(false);
                            break;
                        case 1:
                            _musicAndSounds.DecMusicVolume();
                            break;
                        case 2:
                            _musicAndSounds.DecSoundsVolume();
                            break;
                        case 3:
                            lang.DecSelection();
                            break;
                        default:
                            break;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);
                }
                _keyLeftUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A) && gamePadState.DPad.Left == ButtonState.Released)
                && _keyLeftUp == false)
            {
                _keyLeftUp = true;
            }

            _gameMarkerPosition = 30.0f + (_gameMarkerIndex * 100.0f);

            if ( (keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                && _keyUpUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[_gameMarkerIndex]++;
                    if (gameLogic.currentCode[_gameMarkerIndex] > 9)
                    {
                        gameLogic.currentCode[_gameMarkerIndex] = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);        
                }
                else if (gameState == GameState.Menu)
                {
                    _menuMarkerIndex--;
                    if (_menuMarkerIndex < 0)
                    {
                        _menuMarkerIndex = 4;
                    }
                    if ((_menuMarkerIndex == 2 && saveData.isAttemptsLimit == false) ||
                        (_menuMarkerIndex == 4 && saveData.isTimeLimit == false) ||
                        (_menuMarkerIndex == 6 && _musicAndSounds.GetIsSounding() == false))
                    {
                        _menuMarkerIndex--;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                }
                else if (gameState == GameState.Option)
                {
                    _optionMarkerIndex--;
                    if (_optionMarkerIndex < 0)
                    {
                        _optionMarkerIndex = 3;
                    }
                    if (_optionMarkerIndex == 2 && _musicAndSounds.GetIsSounding() == false)
                    {
                        _optionMarkerIndex -= 2;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                }
                _keyUpUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W) && gamePadState.DPad.Up == ButtonState.Released)
                && _keyUpUp == false)
            {
                _keyUpUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
                && _keyDownUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[_gameMarkerIndex]--;
                    if (gameLogic.currentCode[_gameMarkerIndex] < 0)
                    {
                        gameLogic.currentCode[_gameMarkerIndex] = 9;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);
                }
                else if (gameState == GameState.Menu)
                {
                    _menuMarkerIndex++;
                    if ((_menuMarkerIndex == 2 && saveData.isAttemptsLimit == false) ||
                        (_menuMarkerIndex == 4 && saveData.isTimeLimit == false) ||
                        (_menuMarkerIndex == 6 && _musicAndSounds.GetIsSounding() == false))
                    {
                        _menuMarkerIndex++;
                    }
                    if (_menuMarkerIndex > 4)
                    {
                        _menuMarkerIndex = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                }
                else if (gameState == GameState.Option)
                {
                    _optionMarkerIndex++;
                    if (_optionMarkerIndex == 1 && _musicAndSounds.GetIsSounding() == false)
                    {
                        _optionMarkerIndex += 2;
                    }
                    if (_optionMarkerIndex > 3)
                    {
                        _optionMarkerIndex = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                }
                _keyDownUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
                && _keyDownUp == false)
            {
                _keyDownUp = true;
            }

            _menuMarkerPosition = _menuMarkerStartY + (_menuMarkerIndex * _menuMarkerStepY) - 5;
            _optionMarkerPosition = _optionMarkerStartY + (_optionMarkerIndex * _optionMarkerStepY) - 5;

            if ( (keyboardState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
                && _keySpaceUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    bool isCodeCorrect = gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        gameState = GameState.FinishGame;
                        _musicAndSounds.PlaySoundEffect(_successSoundEffect);
                    }
                    else if (((gameLogic.numberOfAttempts >= saveData.attemptsLimit) && saveData.isAttemptsLimit))
                    {
                        gameState = GameState.FinishGame;
                        _musicAndSounds.PlaySoundEffect(_failSoundEffect);
                    }
                    else
                    {
                        _musicAndSounds.PlaySoundEffect(_unlockTrySoundEffect);
                    }
                }                
                _keySpaceUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Space) && gamePadState.Buttons.A == ButtonState.Released)
                && _keySpaceUp == false)
            {
                _keySpaceUp = true;
            }

            if (gameState == GameState.InGame)
            {
                if (saveData.isTimeLimit)
                {
                    _remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (_remainingTime <= 0)
                    {
                        _remainingTime = 0;
                        gameState = GameState.FinishGame;
                        _musicAndSounds.PlaySoundEffect(_failSoundEffect);
                    }
                }
                else
                {
                    _playingTime += gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            switch (gameState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameTitle), new Vector2(120, 10), Color.White);

                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.StartGameKey), new Vector2(120, 70), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.GameInstuctionKey), new Vector2(120, 100), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.GameOptionKey), new Vector2(120, 130), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.ExitGameKey), new Vector2(120, 160), Color.White);

                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.CodeLength) + saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.IsLimitedAttempts) + lang.GetBoolInLang(saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.NumberAttempts) + saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), saveData.isAttemptsLimit ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.IsTimeLimitation) + lang.GetBoolInLang(saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.TimeLimit) + saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), saveData.isTimeLimit ? Color.White : Color.Gray);

                    _spriteBatch.DrawString(_littleFont, lang.GetLangText(LangKey.CreditsStart) + "Bartłomiej Grywalski", new Vector2(20, 530), Color.White);
                    _spriteBatch.DrawString(_littleFont, lang.GetLangText(LangKey.VersionInfo) + versionText, new Vector2(600, 530), Color.White);
                    _spriteBatch.Draw(_menuMarkerSprite, new Vector2(210, _menuMarkerPosition), Color.White);
                    break;
                case GameState.GameInstructions:
                    _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameInstuction), new Vector2(250, 50), Color.White);

                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(50, 120), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorsOption), new Vector2(50, 150), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorRed), new Vector2(50, 180), Color.Red);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorBlue), new Vector2(50, 210), Color.Blue);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorGreen), new Vector2(50, 240), Color.Green);

                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.ControlsInGame), new Vector2(50, 300), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpLeftRight), new Vector2(50, 330), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpUpDown), new Vector2(50, 360), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpSpace), new Vector2(50, 390), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpEsc), new Vector2(50, 420), Color.White);

                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 500), Color.White);

                    break;
                case GameState.Option:
                    _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameOption), new Vector2(300, 50), Color.White);

                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.PlayingSound) + lang.GetBoolInLang(_musicAndSounds.GetIsSounding()), GetOptionMarkerPosition(0), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.MusicVolume) + _musicAndSounds.GetMusicVolumePercentString(), GetOptionMarkerPosition(1), _musicAndSounds.GetIsSounding() ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.SoundsVolume) + _musicAndSounds.GetSoundsVolumePercentString(), GetOptionMarkerPosition(2), _musicAndSounds.GetIsSounding() ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.LanguageSelected), GetOptionMarkerPosition(3), Color.White);

                    _spriteBatch.Draw(_menuMarkerSprite, new Vector2(210, _optionMarkerPosition), Color.White);

                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 500), Color.White);
                    break;
                case GameState.InGame:
                    for (int i = 0; i < saveData.codeLength; i++)
                    {
                        _spriteBatch.DrawString(_largeFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 150), Color.White);
                    }
                    _spriteBatch.Draw(_gameMarkerSprite, new Vector2(_gameMarkerPosition, 150), Color.White);

                    if (!IS_DEBUG_MODE)
                    {
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(5, 400), Color.White);
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorsOption), new Vector2(5, 430), Color.White);
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorRed), new Vector2(5, 460), Color.Red);
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorBlue), new Vector2(5, 490), Color.Blue);
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.HelpColorGreen), new Vector2(5, 520), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.DebugMarkerIndex) + _gameMarkerIndex.ToString()
                            + lang.GetLangText(LangKey.DebugMarkerPos) + _gameMarkerPosition.ToString(), new Vector2(10, 400), Color.White);
                        _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.DebugCurrentCode) + gameLogic.CurrentCodeString(), new Vector2(10, 430), Color.White);
                    }

                    if (saveData.isAttemptsLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameRemainingAttempts) + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString(), new Vector2(360, 10), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameNumberOfAttempts) + gameLogic.numberOfAttempts.ToString(), new Vector2(360, 10), Color.White);
                    }

                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, string.Format(lang.GetLangText(LangKey.GameRemainingTime), _remainingTime), new Vector2(10, 10), Color.White);
                    }
                    for (int i = 0; i < gameLogic.guessCodeHistory.Count; i++)
                    {
                        for (int j = 0; j < gameLogic.guessCodeHistory[i].Count; j++)
                        {
                            _guessCodeHistoryTextPlace = new Vector2(600 + (j * 40), (i + 2) * 50);
                            _spriteBatch.DrawString(_bigFont, gameLogic.guessCodeHistory[i][j].value.ToString(), _guessCodeHistoryTextPlace, DecodeColor(gameLogic.guessCodeHistory[i][j].digitState));
                        }
                    }
                    break;
                case GameState.FinishGame:
                    if (gameLogic.codeGuessed)
                    {
                        _endGameInfo = lang.GetLangText(LangKey.FinishWin); _endGameColor = Color.Green;
                    }
                    else
                    {
                        _endGameInfo = lang.GetLangText(LangKey.FinishLost); _endGameColor = Color.Red;
                    }

                    _spriteBatch.DrawString(_bigFont, _endGameInfo, new Vector2(200, 40), _endGameColor);

                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_middleFont, string.Format(lang.GetLangText(LangKey.FinishRemainingTime), _remainingTime), new Vector2(200, 100), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_middleFont, string.Format(lang.GetLangText(LangKey.FinishPlayingTime), _playingTime), new Vector2(200, 100), Color.White);
                    }

                    if (saveData.isAttemptsLimit)
                    {
                        _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.FinishRemainingAttempts) + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString(),
                            new Vector2(200, 160), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.FinishNumberOfAttempts) + gameLogic.numberOfAttempts.ToString(), new Vector2(200, 160), Color.White);
                    }

                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.FinishCorrectCode) + gameLogic.CorrectCodeString(), new Vector2(200, 220), Color.White);

                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.FinishPlayAgain), new Vector2(10, 360), Color.White);
                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 400), Color.White);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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
        private void StartNewGame()
        {
            gameLogic = new GameLogic(saveData.codeLength, MAX_NUMBER_OF_HINTS);
            _gameMarkerIndex = 0;
            _playingTime = 0;
            _remainingTime = saveData.timeLimit;
            gameState = GameState.InGame;
        }
        private void GoToMainMenu()
        {
            _menuMarkerIndex = 0;
            gameState = GameState.Menu;
        }
        SaveData saveData = new SaveData();
        private void ReadSaveData()
        { 
            if(System.IO.File.Exists(_saveDataPath))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

                System.IO.StreamReader file = new System.IO.StreamReader(_saveDataPath);
                saveData = (SaveData)reader.Deserialize(file);
                file.Close();
            }
            else
            {
                saveData.codeLength = 4;
                saveData.isAttemptsLimit = false;
                saveData.attemptsLimit = 8;
                saveData.isTimeLimit = false;
                saveData.timeLimit = 30;
                saveData.isSounding = true;
                saveData.musicVolumePercent = 70;
                saveData.soundsVolumePercent = 90;
                saveData.langID = 0;
            }
        }
        protected override void EndRun()
        {
            WriteSaveData();
            base.EndRun();
        }
        private void WriteSaveData()
        {
            saveData.isSounding = _musicAndSounds.GetIsSounding();
            saveData.musicVolumePercent = _musicAndSounds.GetMusicVolumePercent();
            saveData.soundsVolumePercent = _musicAndSounds.GetSoundsVolumePercent();
            saveData.langID = lang.GetLangID();

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

            System.IO.FileStream file = System.IO.File.Create(_saveDataPath);
            writer.Serialize(file, saveData);
            file.Close();
        }
        private Vector2 GetMenuMarkerPosition(int row)
        {
            return new Vector2(_menuMarkerStartX, _menuMarkerStartY + (row * _menuMarkerStepY));
        }
        private Vector2 GetOptionMarkerPosition(int row)
        {
            return new Vector2(_optionMarkerStartX, _optionMarkerStartY + (row * _optionMarkerStepY));
        }
    }
}
