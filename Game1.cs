using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace CodeBreaker_MonoGame
{
    public enum GameState { Menu, GameInstructions, Option,  InGame, FinishGame }
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _largeFont, _bigFont, _middleFont, _smallFont, _littleFont;

        private Texture2D _menuMarkerSprite, _gameMarkerSprite, _squereBaseSprite;
        private Texture2D _attemptIconReady, _attemptIconUsed;
        private Texture2D _iconGame;
        private Texture2D _background;

        private MusicAndSounds _musicAndSounds;

        private SoundEffect _menuUpDownSoundEffect, _menuSideSoundEffect;
        private SoundEffect _startSoundEffect, _returnMenuSoundEffect;
        private SoundEffect _gameUpDownSoundEffect, _gameSideSoundEffect, _unlockTrySoundEffect;
        private SoundEffect _successSoundEffect, _failSoundEffect;
        private SoundEffect _menuNaviInstr, _menuNaviOption;

        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private bool _keyEnterUp, _keyHelpUp, _keyOptionUp, _keyEscapeUp, _keySpaceUp;

        private GameLogic _gameLogic;

        private GameState _gameState;

        private int _menuMarkerIndex;
        private float _menuMarkerPosition;

        private double _playingTime, _remainingTime;

        private int _gameMarkerIndex;
        private float _gameMarkerPosition;

        private Vector2 _guessCodeHistoryTextPlace;

        private string _endGameInfo;
        private Color _endGameColor;

        string _saveDataPath;

        private Lang _lang;

        private int _optionMarkerIndex;
        private float _optionMarkerPosition;

        private Color _tableColor = Color.Black;

        private Rectangle _timeLimitBarBase;

        private int _attemptIconStartX, _attemptIconStartY, _attemptIconStepX, _attmptIconSize;

        private Vector2 _iconLocation;

        SaveData saveData;

        const bool IS_DEBUG_MODE = false;

        const int MAX_NUMBER_OF_HINTS = 8;

        const string VERSION_GAME = "1.4.2 (2022.10.26)";

        const int MENU_MARKER_START_X = 225, MENU_MARKER_START_Y = 330, MENU_MARKER_STEP_Y = 40;

        const int OPTION_MARKER_START_X = 225, OPTION_MARKER_START_Y = 150, PTION_MARKER_STEP_Y = 40;

        const int GUESS_HISTORY_START_X = 600, GUESS_HISTORY_START_Y = 115;
        const int GUESS_HISTORY_STEP_X = 40, GUESS_HISTORY_STEP_Y = 50, GUESS_HISTORY_THICKNESS = 3;
        const int GUESS_TABLE_OFFSET_X = -11, gUESS_TABLE_OFFSET_Y = -3;

        const int CODE_POSITION_START_X = 50, CODE_POSITION_START_Y = 180, CODE_POSITION_STEP_X = 105;
        const int CODE_OFFSET_MARKER_X = -20, CODE_OFFSET_MARKER_Y = -2;

        const int TIME_LIMIT_BAR_START_X = 10 , TIME_LIMIT_BAR_START_Y = 120, TIME_LIMIT_BAR_HEIGHT = 20, TIME_LIMIT_BAR_WIDTH_MAX = 320;
        
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
            ReadSaveData();
            GoToMainMenu();
            _graphics.PreferredBackBufferWidth = 820;
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();

            base.Initialize();
        }
        private void InitializePrivatVariables()
        {
            _saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");

            _keyRightUp = true; _keyLeftUp = true; _keyUpUp = true; _keyDownUp = true;
            _keyEnterUp = true; _keyHelpUp = true; _keyOptionUp = true; _keyEscapeUp = true; _keySpaceUp = true;

            _timeLimitBarBase = new Rectangle(TIME_LIMIT_BAR_START_X, TIME_LIMIT_BAR_START_Y, TIME_LIMIT_BAR_WIDTH_MAX, TIME_LIMIT_BAR_HEIGHT);
            _iconLocation = new Vector2(60, 27);

            _attemptIconStartX = 415; _attemptIconStartY = 65; _attemptIconStepX = 26; _attmptIconSize = 24;
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
            _squereBaseSprite = Content.Load<Texture2D>("sprites/tableBase");
            _attemptIconReady = Content.Load<Texture2D>("sprites/attemptIconReady");
            _attemptIconUsed = Content.Load<Texture2D>("sprites/attemptIconUsed");
            _iconGame = Content.Load<Texture2D>("sprites/icon96");
            _background = Content.Load<Texture2D>("background/background_820x550");

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
                if (_gameState == GameState.Menu || _gameState == GameState.FinishGame)
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
                if (_gameState == GameState.Menu)
                {
                    _musicAndSounds.PlaySoundEffect(_menuNaviInstr);
                    _gameState = GameState.GameInstructions;
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
                if (_gameState == GameState.Menu)
                {
                    _musicAndSounds.PlaySoundEffect(_menuNaviOption);
                    _gameState = GameState.Option;
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
                if (_gameState == GameState.Menu)
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
                if (_gameState == GameState.InGame)
                {
                    _gameMarkerIndex++;
                    if (_gameMarkerIndex > saveData.codeLength - 1)
                    {
                        _gameMarkerIndex = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
                }
                else if (_gameState == GameState.Menu)
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
                else if(_gameState == GameState.Option)
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
                            _lang.IncSelection();
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
                if (_gameState == GameState.InGame)
                {
                    _gameMarkerIndex--;
                    if (_gameMarkerIndex < 0)
                    {
                        _gameMarkerIndex = saveData.codeLength - 1;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
                }
                else if (_gameState == GameState.Menu)
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
                else if (_gameState == GameState.Option)
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
                            _lang.DecSelection();
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
                if (_gameState == GameState.InGame)
                {
                    _gameLogic.currentCode[_gameMarkerIndex]++;
                    if (_gameLogic.currentCode[_gameMarkerIndex] > 9)
                    {
                        _gameLogic.currentCode[_gameMarkerIndex] = 0;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);        
                }
                else if (_gameState == GameState.Menu)
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
                else if (_gameState == GameState.Option)
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
                if (_gameState == GameState.InGame)
                {
                    _gameLogic.currentCode[_gameMarkerIndex]--;
                    if (_gameLogic.currentCode[_gameMarkerIndex] < 0)
                    {
                        _gameLogic.currentCode[_gameMarkerIndex] = 9;
                    }
                    _musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);
                }
                else if (_gameState == GameState.Menu)
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
                else if (_gameState == GameState.Option)
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

            _menuMarkerPosition = MENU_MARKER_START_Y + (_menuMarkerIndex * MENU_MARKER_STEP_Y) - 5;
            _optionMarkerPosition = OPTION_MARKER_START_Y + (_optionMarkerIndex * PTION_MARKER_STEP_Y) - 5;

            if ( (keyboardState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
                && _keySpaceUp == true)
            {
                if (_gameState == GameState.InGame)
                {
                    bool isCodeCorrect = _gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        _gameState = GameState.FinishGame;
                        _musicAndSounds.PlaySoundEffect(_successSoundEffect);
                    }
                    else if (((_gameLogic.numberOfAttempts >= saveData.attemptsLimit) && saveData.isAttemptsLimit))
                    {
                        _gameState = GameState.FinishGame;
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

            if (_gameState == GameState.InGame)
            {
                if (saveData.isTimeLimit)
                {
                    _remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (_remainingTime <= 0)
                    {
                        _remainingTime = 0;
                        _gameState = GameState.FinishGame;
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

            _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
            switch (_gameState)
            {
                case GameState.Menu:
                    _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameTitle), new Vector2(180, 50), Color.Black);
                    _spriteBatch.Draw(_iconGame, _iconLocation, Color.White);

                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.StartGameKey), new Vector2(120, 140), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.GameInstuctionKey), new Vector2(120, 170), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.GameOptionKey), new Vector2(120, 200), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.ExitGameKey), new Vector2(120, 230), Color.Black);

                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.GameModifiers), GetMenuMarkerPosition(-1), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.CodeLength) + saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.IsLimitedAttempts) + _lang.GetBoolInLang(saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.NumberAttempts) + saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), saveData.isAttemptsLimit ? Color.Black : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.IsTimeLimitation) + _lang.GetBoolInLang(saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.TimeLimit) + saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), saveData.isTimeLimit ? Color.Black : Color.Gray);

                    _spriteBatch.DrawString(_littleFont, _lang.GetLangText(LangKey.CreditsStart) + "Bartłomiej Grywalski", new Vector2(20, 530), Color.Black);
                    _spriteBatch.DrawString(_littleFont, _lang.GetLangText(LangKey.VersionInfo) + VERSION_GAME, new Vector2(600, 530), Color.Black);
                    _spriteBatch.Draw(_menuMarkerSprite, new Vector2(210, _menuMarkerPosition), Color.Purple);
                    break;
                case GameState.GameInstructions:
                    _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameInstuction), new Vector2(250, 50), Color.Black);

                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(50, 120), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorsOption), new Vector2(50, 150), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorRed), new Vector2(50, 180), Color.Red);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorBlue), new Vector2(50, 210), Color.Blue);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorGreen), new Vector2(50, 240), Color.Green);

                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.ControlsInGame), new Vector2(50, 300), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpLeftRight), new Vector2(50, 330), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpUpDown), new Vector2(50, 360), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpSpace), new Vector2(50, 390), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpEsc), new Vector2(50, 420), Color.Black);

                    _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 500), Color.Black);

                    break;
                case GameState.Option:
                    _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameOption), new Vector2(300, 50), Color.Black);

                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.PlayingSound) + _lang.GetBoolInLang(_musicAndSounds.GetIsSounding()), GetOptionMarkerPosition(0), Color.Black);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.MusicVolume) + _musicAndSounds.GetMusicVolumePercentString(), GetOptionMarkerPosition(1), _musicAndSounds.GetIsSounding() ? Color.Black : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.SoundsVolume) + _musicAndSounds.GetSoundsVolumePercentString(), GetOptionMarkerPosition(2), _musicAndSounds.GetIsSounding() ? Color.Black : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.LanguageSelected), GetOptionMarkerPosition(3), Color.Black);

                    _spriteBatch.Draw(_menuMarkerSprite, new Vector2(210, _optionMarkerPosition), Color.Purple);

                    _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 500), Color.Black);
                    break;
                case GameState.InGame:
                    for (int i = 0; i < saveData.codeLength; i++)
                    {
                        _spriteBatch.DrawString(_largeFont, _gameLogic.currentCode[i].ToString(), new Vector2(CODE_POSITION_START_X + (CODE_POSITION_STEP_X * i), CODE_POSITION_START_Y), Color.Black);
                        _spriteBatch.Draw(_gameMarkerSprite, new Vector2(CODE_POSITION_START_X + CODE_OFFSET_MARKER_X + (i * CODE_POSITION_STEP_X), CODE_POSITION_START_Y + CODE_OFFSET_MARKER_Y), i == _gameMarkerIndex ? Color.Purple : Color.Gray);
                    }

                    if (!IS_DEBUG_MODE)
                    {
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpSingleDigitOnce), new Vector2(5, 400), Color.Black);
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorsOption), new Vector2(5, 430), Color.Black);
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorRed), new Vector2(5, 460), Color.Red);
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorBlue), new Vector2(5, 490), Color.Blue);
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.HelpColorGreen), new Vector2(5, 520), Color.Green);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.DebugMarkerIndex) + _gameMarkerIndex.ToString()
                            + _lang.GetLangText(LangKey.DebugMarkerPos) + _gameMarkerPosition.ToString(), new Vector2(10, 400), Color.Black);
                        _spriteBatch.DrawString(_smallFont, _lang.GetLangText(LangKey.DebugCurrentCode) + _gameLogic.CurrentCodeString(), new Vector2(10, 430), Color.Black);
                    }

                    if (saveData.isAttemptsLimit)
                    {
                        int remainingAttempt = saveData.attemptsLimit - _gameLogic.numberOfAttempts;
                        _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameRemainingAttempts) + remainingAttempt.ToString(), new Vector2(360, 10), Color.Black);
                        for (int i = 0; i < saveData.attemptsLimit; i++)
                        {
                            if (i < remainingAttempt)
                            {
                                _spriteBatch.Draw(_attemptIconReady, new Rectangle(_attemptIconStartX + (i * _attemptIconStepX), _attemptIconStartY, _attmptIconSize, _attmptIconSize), Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(_attemptIconUsed, new Rectangle(_attemptIconStartX + (i * _attemptIconStepX), _attemptIconStartY, _attmptIconSize, _attmptIconSize), Color.White);
                            }
                        }
                    }
                    else
                    {
                        _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameNumberOfAttempts) + _gameLogic.numberOfAttempts.ToString(), new Vector2(360, 10), Color.Black);
                    }

                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, string.Format(_lang.GetLangText(LangKey.GameRemainingTime), _remainingTime), new Vector2(10, 10), Color.Black);
                        _spriteBatch.Draw(_squereBaseSprite, _timeLimitBarBase, Color.White);
                        _spriteBatch.Draw(_squereBaseSprite, GetTimieLimitBarRectangel(), GetTimeLimitBarColor());
                    }
                    
                    for (int i = 0; i < _gameLogic.guessCodeHistory.Count; i++)
                    {
                        for (int j = 0; j < _gameLogic.guessCodeHistory[i].Count; j++)
                        {
                            _guessCodeHistoryTextPlace = new Vector2(GUESS_HISTORY_START_X + (j * GUESS_HISTORY_STEP_X), GUESS_HISTORY_START_Y + i * GUESS_HISTORY_STEP_Y);
                            _spriteBatch.DrawString(_bigFont, _gameLogic.guessCodeHistory[i][j].value.ToString(), _guessCodeHistoryTextPlace, DecodeColor(_gameLogic.guessCodeHistory[i][j].digitState));
                        }
                    }

                    if (_gameLogic.guessCodeHistory.Count > 0)
                    {
                        for (int i = 0; i < _gameLogic.guessCodeHistory.Count; i++)
                        {
                            _spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X, (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y) + i * GUESS_HISTORY_STEP_Y,
                                                GUESS_HISTORY_STEP_X * _gameLogic.guessCodeHistory[0].Count, GUESS_HISTORY_THICKNESS), _tableColor);
                            if (i == _gameLogic.guessCodeHistory.Count - 1)
                            {
                                _spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X, (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y) + (i + 1) * GUESS_HISTORY_STEP_Y,
                                                    GUESS_HISTORY_STEP_X * _gameLogic.guessCodeHistory[0].Count, GUESS_HISTORY_THICKNESS), _tableColor);
                            }
                        }
                        for (int i = 0; i < _gameLogic.guessCodeHistory[0].Count; i++)
                        {
                            _spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X + (i * GUESS_HISTORY_STEP_X), (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y),
                                                GUESS_HISTORY_THICKNESS, GUESS_HISTORY_STEP_Y * _gameLogic.guessCodeHistory.Count), _tableColor);
                            if (i == _gameLogic.guessCodeHistory[0].Count - 1)
                            {
                                _spriteBatch.Draw(_squereBaseSprite, new Rectangle(GUESS_HISTORY_START_X + GUESS_TABLE_OFFSET_X + ((i + 1) * GUESS_HISTORY_STEP_X), (GUESS_HISTORY_START_Y + gUESS_TABLE_OFFSET_Y),
                                                GUESS_HISTORY_THICKNESS, (GUESS_HISTORY_STEP_Y * _gameLogic.guessCodeHistory.Count) + GUESS_HISTORY_THICKNESS), _tableColor);
                            }
                        }
                    }

                    break;
                case GameState.FinishGame:
                    if (_gameLogic.codeGuessed)
                    {
                        _endGameInfo = _lang.GetLangText(LangKey.FinishWin); _endGameColor = Color.Green;
                    }
                    else
                    {
                        _endGameInfo = _lang.GetLangText(LangKey.FinishLost); _endGameColor = Color.Red;
                    }

                    _spriteBatch.DrawString(_bigFont, _endGameInfo, new Vector2(200, 40), _endGameColor);

                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_middleFont, string.Format(_lang.GetLangText(LangKey.FinishRemainingTime), _remainingTime), new Vector2(200, 100), Color.Black);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_middleFont, string.Format(_lang.GetLangText(LangKey.FinishPlayingTime), _playingTime), new Vector2(200, 100), Color.Black);
                    }

                    if (saveData.isAttemptsLimit)
                    {
                        _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.FinishRemainingAttempts) + (saveData.attemptsLimit - _gameLogic.numberOfAttempts).ToString(),
                            new Vector2(200, 160), Color.Black);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.FinishNumberOfAttempts) + _gameLogic.numberOfAttempts.ToString(), new Vector2(200, 160), Color.Black);
                    }

                    _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.FinishCorrectCode) + _gameLogic.CorrectCodeString(), new Vector2(200, 220), Color.Black);

                    _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.FinishPlayAgain), new Vector2(10, 360), Color.Black);
                    _spriteBatch.DrawString(_middleFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 400), Color.Black);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private Rectangle GetTimieLimitBarRectangel()
        {
            float barWidth = ((float)_remainingTime / (float)saveData.timeLimit) * TIME_LIMIT_BAR_WIDTH_MAX;
            return new Rectangle(TIME_LIMIT_BAR_START_X, TIME_LIMIT_BAR_START_Y, (int)barWidth, TIME_LIMIT_BAR_HEIGHT);
        }

        private Color GetTimeLimitBarColor()
        {
            float remainingTimeFraction = (float)_remainingTime / (float)saveData.timeLimit;
            Color barColor;
            if (remainingTimeFraction >= 0.8f)
            {
                barColor = new Color(44, 186, 0);
            }
            else if(remainingTimeFraction >= 0.6f && remainingTimeFraction < 0.8f)
            {
                barColor = new Color(163, 255, 0);
            }
            else if(remainingTimeFraction >= 0.4f && remainingTimeFraction < 0.6f)
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
        private void StartNewGame()
        {
            _gameLogic = new GameLogic(saveData.codeLength, MAX_NUMBER_OF_HINTS);
            _gameMarkerIndex = 0;
            _playingTime = 0;
            _remainingTime = saveData.timeLimit;
            if (saveData.isAttemptsLimit)
            {
                if (saveData.attemptsLimit < 10)
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
            _gameState = GameState.InGame;
        }
        private void GoToMainMenu()
        {
            _menuMarkerIndex = 0;
            _gameState = GameState.Menu;
        }
        
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
            _lang = new Lang(saveData.langID);
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
            saveData.langID = _lang.GetLangID();

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

            System.IO.FileStream file = System.IO.File.Create(_saveDataPath);
            writer.Serialize(file, saveData);
            file.Close();
        }
        private Vector2 GetMenuMarkerPosition(int row)
        {
            return new Vector2(MENU_MARKER_START_X, MENU_MARKER_START_Y + (row * MENU_MARKER_STEP_Y));
        }
        private Vector2 GetOptionMarkerPosition(int row)
        {
            return new Vector2(OPTION_MARKER_START_X, OPTION_MARKER_START_Y + (row * PTION_MARKER_STEP_Y));
        }
    }
}
