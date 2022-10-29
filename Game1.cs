using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using CodeBreaker_MonoGame.Screen;

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

        public MusicAndSounds musicAndSounds;

        private SoundEffect _menuUpDownSoundEffect, _menuSideSoundEffect;
        private SoundEffect _startSoundEffect, _returnMenuSoundEffect;
        private SoundEffect _gameUpDownSoundEffect, _gameSideSoundEffect, _unlockTrySoundEffect;
        private SoundEffect _successSoundEffect, _failSoundEffect;
        private SoundEffect _menuNaviInstr, _menuNaviOption;

        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private bool _keyEscapeUp, _keySpaceUp;

        private GameLogic _gameLogic;

        private GameState _gameState;

        private double _playingTime, _remainingTime;

        private int _gameMarkerIndex;
        private float _gameMarkerPosition;

        private Vector2 _guessCodeHistoryTextPlace;

        string _saveDataPath;

        private Lang _lang;

        private Color _tableColor = Color.Black;

        private Rectangle _timeLimitBarBase;

        private int _attemptIconStartX, _attemptIconStartY, _attemptIconStepX, _attmptIconSize;

        SaveData _saveData;

        const bool IS_DEBUG_MODE = true;

        const int MAX_NUMBER_OF_HINTS = 8;

        const string VERSION_GAME = "1.4.6 (2022.10.29)";

        const int GUESS_HISTORY_START_X = 600, GUESS_HISTORY_START_Y = 115;
        const int GUESS_HISTORY_STEP_X = 40, GUESS_HISTORY_STEP_Y = 50, GUESS_HISTORY_THICKNESS = 3;
        const int GUESS_TABLE_OFFSET_X = -11, gUESS_TABLE_OFFSET_Y = -3;

        const int CODE_POSITION_START_X = 50, CODE_POSITION_START_Y = 180, CODE_POSITION_STEP_X = 105;
        const int CODE_OFFSET_MARKER_X = -20, CODE_OFFSET_MARKER_Y = -2;

        const int TIME_LIMIT_BAR_START_X = 10 , TIME_LIMIT_BAR_START_Y = 120, TIME_LIMIT_BAR_HEIGHT = 20, TIME_LIMIT_BAR_WIDTH_MAX = 320;

        MenuScene _menuScene;
        InstructionScene _instructionScene;
        OptionScene _optionScene;

        FinishScene _finishScene;

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
            _graphics.PreferredBackBufferWidth = 820;
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();

            base.Initialize();
        }
        private void InitializePrivatVariables()
        {
            _saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");

            _keyRightUp = true; _keyLeftUp = true; _keyUpUp = true; _keyDownUp = true;
            _keyEscapeUp = true; _keySpaceUp = true;

            _timeLimitBarBase = new Rectangle(TIME_LIMIT_BAR_START_X, TIME_LIMIT_BAR_START_Y, TIME_LIMIT_BAR_WIDTH_MAX, TIME_LIMIT_BAR_HEIGHT);

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
            musicAndSounds = new MusicAndSounds(song, _saveData);
            musicAndSounds.Play();

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

            GoToMainMenu(false);

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            switch (_gameState)
            {
                case GameState.Menu:
                    _menuScene.Update();
                    break;
                case GameState.GameInstructions:
                    _instructionScene.Update();
                    break;
                case GameState.Option:
                    _optionScene.Update();
                    break;
                case GameState.InGame:
                    break;
                case GameState.FinishGame:
                    _finishScene.Update();
                    break;
                default:
                    break;
            }               

            //if ( (keyboardState.IsKeyDown(Keys.Enter) || gamePadState.Buttons.Start == ButtonState.Pressed)
            //    && _keyEnterUp == true)
            //{
            //    if (_gameState == GameState.FinishGame)
            //    {
            //        GoToGame();
            //    }
            //    _keyEnterUp = false;
            //}
            //else if ( (keyboardState.IsKeyUp(Keys.Enter) && gamePadState.Buttons.Start == ButtonState.Released)
            //    && _keyEnterUp == false)
            //{
            //    _keyEnterUp = true;
            //}

            if ( (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                && _keyEscapeUp == true)
            {
                if (_gameState == GameState.InGame/* || _gameState == GameState.FinishGame*/)
                {                    
                    GoToMainMenu(true);
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
                    if (_gameMarkerIndex > _saveData.codeLength - 1)
                    {
                        _gameMarkerIndex = 0;
                    }
                    musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
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
                        _gameMarkerIndex = _saveData.codeLength - 1;
                    }
                    musicAndSounds.PlaySoundEffect(_gameSideSoundEffect);
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
                    musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);        
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
                    musicAndSounds.PlaySoundEffect(_gameUpDownSoundEffect);
                }
                _keyDownUp = false;
            }
            else if ( (keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
                && _keyDownUp == false)
            {
                _keyDownUp = true;
            }

            if ( (keyboardState.IsKeyDown(Keys.Space) || gamePadState.Buttons.A == ButtonState.Pressed)
                && _keySpaceUp == true)
            {
                if (_gameState == GameState.InGame)
                {
                    bool isCodeCorrect = _gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        GoToFinish(_successSoundEffect);
                    }
                    else if (((_gameLogic.numberOfAttempts >= _saveData.attemptsLimit) && _saveData.isAttemptsLimit))
                    {
                        GoToFinish(_failSoundEffect);
                    }
                    else
                    {
                        musicAndSounds.PlaySoundEffect(_unlockTrySoundEffect);
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
                if (_saveData.isTimeLimit)
                {
                    _remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (_remainingTime <= 0)
                    {
                        _remainingTime = 0;
                        GoToFinish(_failSoundEffect);
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
                    _menuScene.Draw(_spriteBatch);
                    break;
                case GameState.GameInstructions:
                    _instructionScene.Draw(_spriteBatch);
                    break;
                case GameState.Option:
                    _optionScene.Draw(_spriteBatch);
                    break;
                case GameState.InGame:
                    for (int i = 0; i < _saveData.codeLength; i++)
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

                    if (_saveData.isAttemptsLimit)
                    {
                        int remainingAttempt = _saveData.attemptsLimit - _gameLogic.numberOfAttempts;
                        _spriteBatch.DrawString(_bigFont, _lang.GetLangText(LangKey.GameRemainingAttempts) + remainingAttempt.ToString(), new Vector2(360, 10), Color.Black);
                        for (int i = 0; i < _saveData.attemptsLimit; i++)
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

                    if (_saveData.isTimeLimit)
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
                    _finishScene.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
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
        public void GoToMainMenu(bool isPlaySoundEfect)
        {
            _gameState = GameState.Menu;
            _menuScene = new MenuScene(this, _bigFont, _smallFont, _smallFont, _littleFont, _lang, _iconGame, VERSION_GAME,
                _menuMarkerSprite, _saveData, _menuSideSoundEffect, _menuUpDownSoundEffect);
            _keyEscapeUp = false;
            if (isPlaySoundEfect)
                musicAndSounds.PlaySoundEffect(_returnMenuSoundEffect);
        }
        public void GoToInstuction()
        {
            _instructionScene = new InstructionScene(this, _bigFont, _smallFont, _middleFont, _lang);
            musicAndSounds.PlaySoundEffect(_menuNaviInstr);
            _gameState = GameState.GameInstructions;
        }
        public void GoToOption()
        {
            _optionScene = new OptionScene(this, _menuMarkerSprite, _bigFont, _smallFont, _middleFont, _lang,
                musicAndSounds, _menuSideSoundEffect, _menuUpDownSoundEffect);
            musicAndSounds.PlaySoundEffect(_menuNaviOption);
            _gameState = GameState.Option;
        }
        public void GoToGame()
        {
            _gameLogic = new GameLogic(_saveData.codeLength, MAX_NUMBER_OF_HINTS);
            _gameMarkerIndex = 0;
            _playingTime = 0;
            _remainingTime = _saveData.timeLimit;
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
            _gameState = GameState.InGame;
            musicAndSounds.PlaySoundEffect(_startSoundEffect);
        }
        private void GoToFinish(SoundEffect soundEffect)
        {
            musicAndSounds.PlaySoundEffect(soundEffect);
            _gameState = GameState.FinishGame;
            double time;

            if (_saveData.isTimeLimit)
                time = _remainingTime;
            else
                time = _playingTime;

            _finishScene = new FinishScene(this, _gameLogic, _saveData, _bigFont, _middleFont, _lang, time);
        }
        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            musicAndSounds.PlaySoundEffect(soundEffect);
        }
        private void ReadSaveData()
        { 
            if(System.IO.File.Exists(_saveDataPath))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

                System.IO.StreamReader file = new System.IO.StreamReader(_saveDataPath);
                _saveData = (SaveData)reader.Deserialize(file);
                file.Close();
            }
            else
            {
                _saveData = new SaveData();
                _saveData.codeLength = 4;
                _saveData.isAttemptsLimit = false;
                _saveData.attemptsLimit = 8;
                _saveData.isTimeLimit = false;
                _saveData.timeLimit = 30;
                _saveData.isSounding = true;
                _saveData.musicVolumePercent = 70;
                _saveData.soundsVolumePercent = 90;
                _saveData.langID = 0;
            }
            _lang = new Lang(_saveData.langID);
        }
        protected override void EndRun()
        {
            WriteSaveData();
            base.EndRun();
        }
        private void WriteSaveData()
        {
            _saveData.isSounding = musicAndSounds.GetIsSounding();
            _saveData.musicVolumePercent = musicAndSounds.GetMusicVolumePercent();
            _saveData.soundsVolumePercent = musicAndSounds.GetSoundsVolumePercent();
            _saveData.langID = _lang.GetLangID();

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

            System.IO.FileStream file = System.IO.File.Create(_saveDataPath);
            writer.Serialize(file, _saveData);
            file.Close();
        }
    }
}
