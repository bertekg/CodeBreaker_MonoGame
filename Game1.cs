using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;

namespace CodeBreaker_MonoGame
{
    public enum GameState { Menu, InGame, FinishGame}
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _largeFont, _bigFont, _middleFont, _smallFont;

        private Texture2D _menuMarkerSprite, _gameMarkerSprite;

        private BackgroundSong _backgroundSong;

        private SoundEffect _menuUpDownSoundEffect, _menuSideSoundEffect;
        private SoundEffect _startSoundEffect, _returnMenuSoundEffect;
        private SoundEffect _gameUpDownSoundEffect, _gameSideSoundEffect, _unlockTrySoundEffect;
        private SoundEffect _successSoundEffect, _failSoundEffect;

        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private bool _keyEnterUp, _keyEscapeUp, _keySpaceUp;

        GameLogic gameLogic;

        const bool IS_DEBUG_MODE = true;

        GameState gameState;

        private int _menuMarkerIndex;
        private float menuMarkerPosition;

        private double _playingTime, _remainingTime;

        private int _gameMarkerIndex;
        private float _gameMarketPosition;

        const int MAX_NUMBER_OF_HINTS = 8;

        private Vector2 _guessCodeHistoryTextPlace;

        private string _endGameInfo;
        private Color _endGameColor;

        string _saveDataPath;

        private Lang lang;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            InitializePrivatVariables();
            GoToMainMenu();
            ReadSaveData();
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();
            lang = new Lang(saveData.langID);

            base.Initialize();
        }
        private void InitializePrivatVariables()
        {
            _saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");

            _keyRightUp = true; _keyLeftUp = true; _keyUpUp = true; _keyDownUp = true;
            _keyEnterUp = true; _keyEscapeUp = true; _keySpaceUp = true;
    }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _largeFont = Content.Load<SpriteFont>("fonts/large");
            _bigFont = Content.Load<SpriteFont>("fonts/big");
            _middleFont = Content.Load<SpriteFont>("fonts/middle");
            _smallFont = Content.Load<SpriteFont>("fonts/small");

            _menuMarkerSprite = Content.Load<Texture2D>("sprites/menuMarker");
            _gameMarkerSprite = Content.Load<Texture2D>("sprites/gameMarker");

            Song song = Content.Load<Song>("audio/music");
            _backgroundSong = new BackgroundSong(song, saveData);
            _backgroundSong.Play();

            _menuUpDownSoundEffect = Content.Load<SoundEffect>("sounds/menuUpDown");
            _menuSideSoundEffect = Content.Load<SoundEffect>("sounds/menuSide");
            _startSoundEffect = Content.Load<SoundEffect>("sounds/start");
            _returnMenuSoundEffect = Content.Load<SoundEffect>("sounds/return");
            _gameUpDownSoundEffect = Content.Load<SoundEffect>("sounds/gameUpDown");
            _gameSideSoundEffect = Content.Load<SoundEffect>("sounds/gameSide");
            _unlockTrySoundEffect = Content.Load<SoundEffect>("sounds/unlockTry");
            _successSoundEffect = Content.Load<SoundEffect>("sounds/success");
            _failSoundEffect = Content.Load<SoundEffect>("sounds/fail");
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && _keyEscapeUp == true)
            {
                if (gameState == GameState.InGame || gameState == GameState.FinishGame)
                {
                    PlaySoundEffect(_returnMenuSoundEffect);
                    GoToMainMenu();
                }
                else
                {
                    Exit();
                }
                _keyEscapeUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Escape) && _keyEscapeUp == false)
            {
                _keyEscapeUp = true;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && _keyEnterUp == true)
            {                
                if (gameState == GameState.Menu || gameState == GameState.FinishGame)
                {
                    PlaySoundEffect(_startSoundEffect);
                    StartNewGame();
                }
                _keyEnterUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Enter) && _keyEnterUp == false)
            {
                _keyEnterUp = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && _keyRightUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    _gameMarkerIndex++;
                    if (_gameMarkerIndex > saveData.codeLength - 1)
                    {
                        _gameMarkerIndex = 0;
                    }
                    PlaySoundEffect(_gameSideSoundEffect);
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
                        case 5:
                            _backgroundSong.NewIsSounding(true);
                            break;
                        case 6:
                            _backgroundSong.IncVolume();
                            break;
                        case 7:
                            lang.IncSelection();
                            break;
                        default:
                            break;
                    }
                    PlaySoundEffect(_menuSideSoundEffect);
                }
                _keyRightUp = false;
            }
            else if(keyboardState.IsKeyUp(Keys.Right) && _keyRightUp == false)
            {
                _keyRightUp = true;
            }

            if (keyboardState.IsKeyDown(Keys.Left) && _keyLeftUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    _gameMarkerIndex--;
                    if (_gameMarkerIndex < 0)
                    {
                        _gameMarkerIndex = saveData.codeLength - 1;
                    }
                    PlaySoundEffect(_gameSideSoundEffect);
                }
                else if (gameState == GameState.Menu)
                {
                    switch (_menuMarkerIndex)
                    {
                        case 0:
                            if (saveData.codeLength > 3 )
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
                        case 5:
                            _backgroundSong.NewIsSounding(false);
                            break;
                        case 6:
                            _backgroundSong.DecVolume();
                            break;
                        case 7:
                            lang.DecSelection();
                            break;
                        default:
                            break;
                    }
                    PlaySoundEffect(_menuSideSoundEffect);
                }
                _keyLeftUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Left) && _keyLeftUp == false)
            {
                _keyLeftUp = true;
            }

            _gameMarketPosition = 35.0f + (_gameMarkerIndex * 100.0f);

            if (keyboardState.IsKeyDown(Keys.Up) && _keyUpUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[_gameMarkerIndex]++;
                    if (gameLogic.currentCode[_gameMarkerIndex] > 9)
                    {
                        gameLogic.currentCode[_gameMarkerIndex] = 0;
                    }
                    PlaySoundEffect(_gameUpDownSoundEffect);        
                }
                else if (gameState == GameState.Menu)
                {
                    _menuMarkerIndex--;
                    if (_menuMarkerIndex < 0)
                    {
                        _menuMarkerIndex = 7;
                    }
                    if ((_menuMarkerIndex == 2 && saveData.isAttemptsLimit == false) ||
                        (_menuMarkerIndex == 4 && saveData.isTimeLimit == false) ||
                        (_menuMarkerIndex == 6 && _backgroundSong.GetIsSounding() == false))
                    {
                        _menuMarkerIndex--;
                    }
                    PlaySoundEffect(_menuUpDownSoundEffect);
                }
                _keyUpUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Up) && _keyUpUp == false)
            {
                _keyUpUp = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && _keyDownUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[_gameMarkerIndex]--;
                    if (gameLogic.currentCode[_gameMarkerIndex] < 0)
                    {
                        gameLogic.currentCode[_gameMarkerIndex] = 9;
                    }
                    PlaySoundEffect(_gameUpDownSoundEffect);
                }
                else if (gameState == GameState.Menu)
                {
                    _menuMarkerIndex++;
                    if ((_menuMarkerIndex == 2 && saveData.isAttemptsLimit == false) ||
                        (_menuMarkerIndex == 4 && saveData.isTimeLimit == false) ||
                        (_menuMarkerIndex == 6 && _backgroundSong.GetIsSounding() == false))
                    {
                        _menuMarkerIndex++;
                    }
                    if (_menuMarkerIndex > 7)
                    {
                        _menuMarkerIndex = 0;
                    }
                    PlaySoundEffect(_menuUpDownSoundEffect);
                }
                _keyDownUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Down) && _keyDownUp == false)
            {
                _keyDownUp = true;
            }

            menuMarkerPosition = 195.0f + (_menuMarkerIndex * 40.0f);

            if (keyboardState.IsKeyDown(Keys.Space) && _keySpaceUp == true)
            {
                if (gameState == GameState.InGame)
                {
                    bool isCodeCorrect = gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        gameState = GameState.FinishGame;
                        PlaySoundEffect(_successSoundEffect);
                    }
                    else if (((gameLogic.numberOfAttempts >= saveData.attemptsLimit) && saveData.isAttemptsLimit))
                    {
                        gameState = GameState.FinishGame;
                        PlaySoundEffect(_failSoundEffect);
                    }
                    else
                    {
                        PlaySoundEffect(_unlockTrySoundEffect);
                    }
                }                
                _keySpaceUp = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && _keySpaceUp == false)
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
                        PlaySoundEffect(_failSoundEffect);
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
                    _spriteBatch.DrawString(_bigFont, lang.GetLangText(LangKey.GameTitle), new Vector2(120, 40), Color.White);
                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.StartGameKey), new Vector2(200, 100), Color.White);
                    _spriteBatch.DrawString(_middleFont, lang.GetLangText(LangKey.ExitGameKey), new Vector2(280, 140), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.CodeLength) + saveData.codeLength.ToString(), new Vector2(250, 200), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.IsLimitedAttempts)  + lang.GetBoolInLang(saveData.isAttemptsLimit), new Vector2(250, 240), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.NumberAttempts) + saveData.attemptsLimit.ToString(), new Vector2(250, 280), saveData.isAttemptsLimit ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.IsTimeLimitation) + lang.GetBoolInLang(saveData.isTimeLimit), new Vector2(250, 320), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.TimeLimit) + saveData.timeLimit.ToString(), new Vector2(250, 360), saveData.isTimeLimit ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.PlayingSound) + lang.GetBoolInLang(_backgroundSong.GetIsSounding()), new Vector2(250, 400), Color.White);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.MusicVolume) + _backgroundSong.GetVolumePercentString(), new Vector2(250, 440), _backgroundSong.GetIsSounding() ? Color.White : Color.Gray);
                    _spriteBatch.DrawString(_smallFont, lang.GetLangText(LangKey.LanguageSelected), new Vector2(250, 480), Color.White);
                    _spriteBatch.Draw(_menuMarkerSprite, new Vector2(225, menuMarkerPosition), Color.White);
                    break;
                case GameState.InGame:
                    for (int i = 0; i < saveData.codeLength; i++)
                    {
                        _spriteBatch.DrawString(_largeFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 110), Color.White);
                    }
                    _spriteBatch.Draw(_gameMarkerSprite, new Vector2(_gameMarketPosition, 110), Color.White);

                    if (!IS_DEBUG_MODE)
                    {
                        _spriteBatch.DrawString(_smallFont, "[Left],[Right] - Move the cursor in the code.", new Vector2(5, 270), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[Up],[Down] - Change the value of the indicated digit.", new Vector2(5, 300), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[Space] - Check current code.", new Vector2(5, 330), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[Esc] - Return to main menu.", new Vector2(5, 360), Color.White);
                        _spriteBatch.DrawString(_smallFont, "A single digit can occur only once in the code!", new Vector2(5, 390), Color.White);
                        _spriteBatch.DrawString(_smallFont, "Color codes for individual digits:", new Vector2(5, 420), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[RED] - Not present in the code.", new Vector2(5, 450), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[BLUE] - Appears in the code in a different position.", new Vector2(5, 480), Color.White);
                        _spriteBatch.DrawString(_smallFont, "[GREEN] - Is in the correct position.", new Vector2(5, 510), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_smallFont, "Frame index: " + _gameMarkerIndex.ToString() + ", Frame pos: " + _gameMarketPosition.ToString(), new Vector2(10, 350), Color.White);
                        _spriteBatch.DrawString(_smallFont, "Current code: " + gameLogic.CurrentCodeString(), new Vector2(10, 380), Color.White);
                    }
                    if (saveData.isAttemptsLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, "Remaining Attempts:\n" + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString(), new Vector2(360, 10), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_bigFont, "Number of Attempts:\n" + gameLogic.numberOfAttempts.ToString(), new Vector2(360, 10), Color.White);
                    }
                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, string.Format("Remaining Time:\n{0:N1}", _remainingTime), new Vector2(10, 10), Color.White);
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
                        _endGameInfo = "You WIN!!!"; _endGameColor = Color.Green;
                    }
                    else
                    {
                        _endGameInfo = "You LOSE!!!"; _endGameColor = Color.Red;
                    }

                    _spriteBatch.DrawString(_bigFont, _endGameInfo, new Vector2(280, 40), _endGameColor);
                    if (saveData.isTimeLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, string.Format("Remaining time {0:N3} seconds", _remainingTime), new Vector2(100, 100), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_bigFont, string.Format("Playing time {0:N3} seconds", _playingTime), new Vector2(100, 100), Color.White);
                    }
                    if (saveData.isAttemptsLimit)
                    {
                        _spriteBatch.DrawString(_bigFont, "Remaining Attempts: " + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString(), new Vector2(200, 160), Color.White);
                    }
                    else
                    {
                        _spriteBatch.DrawString(_bigFont, "Number of Attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(200, 160), Color.White);
                    }
                    _spriteBatch.DrawString(_bigFont, "Correct code: " + gameLogic.CorrectCodeString(), new Vector2(220, 220), Color.White);
                    _spriteBatch.DrawString(_bigFont, "Press [Enter] to Start Game again", new Vector2(80, 360), Color.White);
                    _spriteBatch.DrawString(_bigFont, "Press [Esc] to go back Main Menu", new Vector2(75, 420), Color.White);
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
            saveData.isSounding = _backgroundSong.GetIsSounding();
            saveData.musicVolumePercent = _backgroundSong.GetVolumePercent();
            saveData.langID = lang.GetLangID();

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

            System.IO.FileStream file = System.IO.File.Create(_saveDataPath);
            writer.Serialize(file, saveData);
            file.Close();
        }
        private void PlaySoundEffect(SoundEffect soundEffect)
        {
            if (_backgroundSong.GetIsSounding())
            {
                soundEffect.Play(1f, 0.5f, 0f);
            }
        }
    }
}
