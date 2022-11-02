using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using CodeBreaker_MonoGame.Scene;
using CodeBreaker_MonoGame.Class;

namespace CodeBreaker_MonoGame
{
    public enum GameState { Menu, Modifiers, Instructions, Options,  Game, Finish }
    public class Game1 : Game
    {
        public static int ScreenWidth = 820;
        public static int ScreenHeight = 550;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private SpriteFont _largeFont, _bigFont, _middleFont, _smallFont, _littleFont;

        private Texture2D _menuMarkerSprite, _gameMarkerSprite, _squereBaseSprite;
        private Texture2D _attemptIconReady, _attemptIconUsed, _iconGame, _background;
        private Texture2D _buttonSprite, _buttonSprite_30x30, _buttonSprite_230x30, 
            _buttonSprite_80x30, _buttonSprite_30x125;

        public MusicAndSounds musicAndSounds;

        private SoundEffect _menuUpDownSoundEffect, _menuSideSoundEffect, _startSoundEffect;
        private SoundEffect _returnMenuSoundEffect, _gameUpDownSoundEffect, _gameSideSoundEffect;
        private SoundEffect _unlockTrySoundEffect, _successSoundEffect, _failSoundEffect;
        private SoundEffect _menuNaviInstr, _menuNaviOption, _menuNaviModifiers;

        private GameLogic _gameLogic;

        private GameState _gameState;

        string _saveDataPath;

        private Lang _lang;

        SaveData _saveData;

        const bool IS_DEBUG_MODE_FROM_CODE = false;
        private bool _isDebugMode;

        const int MAX_NUMBER_OF_HINTS = 7;

        const string VERSION_GAME = "1.5.5 (2022.11.02)";

        MenuScene _menuScene;
        ModifiersScene _modifiersScene;
        InstructionScene _instructionScene;
        OptionScene _optionScene;
        GameScene _gameScene;
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
            _saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");
            ReadSaveData();
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            //_graphics.IsFullScreen = true;
            //_graphics.ToggleFullScreen();
            _graphics.ApplyChanges();

            base.Initialize();
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
            _buttonSprite = Content.Load<Texture2D>("sprites/button");
            _buttonSprite_30x30 = Content.Load<Texture2D>("sprites/button_30_30");
            _buttonSprite_230x30 = Content.Load<Texture2D>("sprites/button_230_30");
            _buttonSprite_80x30 = Content.Load<Texture2D>("sprites/button_80_30");
            _buttonSprite_30x125 = Content.Load<Texture2D>("sprites/button_30_125");

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
            _menuNaviModifiers = Content.Load<SoundEffect>("sounds/menuNaviModifiers");

            GoToMainMenu(false);

            base.LoadContent();
        }
        protected override void Update(GameTime gameTime)
        {
            switch (_gameState)
            {
                case GameState.Menu:
                    _menuScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.Modifiers:
                    _modifiersScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.Instructions:
                    _instructionScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.Options:
                    _optionScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.Game:
                    _gameScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                case GameState.Finish:
                    _finishScene.Update(gameTime.ElapsedGameTime.TotalSeconds);
                    break;
                default:
                    break;
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
                case GameState.Modifiers:
                    _modifiersScene.Draw(_spriteBatch);
                    break;
                case GameState.Instructions:
                    _instructionScene.Draw(_spriteBatch);
                    break;
                case GameState.Options:
                    _optionScene.Draw(_spriteBatch);
                    break;
                case GameState.Game:
                    _gameScene.Draw(_spriteBatch);
                    break;
                case GameState.Finish:
                    _finishScene.Draw(_spriteBatch);
                    break;
                default:
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
        public void GoToMainMenu(bool isPlaySoundEfect)
        {
            _gameState = GameState.Menu;
            _menuScene = new MenuScene(this, _bigFont, _smallFont, _smallFont, _littleFont, _lang, _iconGame, VERSION_GAME,
                _isDebugMode, _buttonSprite_230x30);
            if (isPlaySoundEfect)
                musicAndSounds.PlaySoundEffect(_returnMenuSoundEffect);
        }
        public void GoToModifiers()
        {
            _gameState = GameState.Modifiers;
            _modifiersScene = new ModifiersScene(this, _bigFont, _smallFont, _smallFont, _lang, _menuMarkerSprite, _saveData,
                _menuSideSoundEffect, _menuUpDownSoundEffect, _buttonSprite, _buttonSprite_30x30);
            musicAndSounds.PlaySoundEffect(_menuNaviModifiers);
        }
        public void GoToInstuction()
        {
            _gameState = GameState.Instructions;
            _instructionScene = new InstructionScene(this, _bigFont, _smallFont, _smallFont, _lang, _buttonSprite);
            musicAndSounds.PlaySoundEffect(_menuNaviInstr);
        }
        public void GoToOption()
        {
            _gameState = GameState.Options;
            _optionScene = new OptionScene(this, _menuMarkerSprite, _bigFont, _smallFont, _smallFont, _lang,
                musicAndSounds, _menuSideSoundEffect, _menuUpDownSoundEffect, _buttonSprite, _buttonSprite_30x30);
            musicAndSounds.PlaySoundEffect(_menuNaviOption);
        }
        public void GoToGame()
        {
            _gameState = GameState.Game;
            _gameLogic = new GameLogic(_saveData.codeLength, MAX_NUMBER_OF_HINTS);
            musicAndSounds.PlaySoundEffect(_startSoundEffect);
            _gameScene = new GameScene(this, _isDebugMode, _saveData, _largeFont, _smallFont, _bigFont, _gameLogic,
                _lang, _gameMarkerSprite, _attemptIconReady, _attemptIconUsed, _squereBaseSprite, _gameSideSoundEffect,
                _gameUpDownSoundEffect, _unlockTrySoundEffect, _successSoundEffect, _failSoundEffect, _buttonSprite,
                _buttonSprite_80x30, _buttonSprite_30x125);
        }
        public void GoToFinish(SoundEffect soundEffect)
        {
            _gameState = GameState.Finish;
            musicAndSounds.PlaySoundEffect(soundEffect);

            double time;
            if (_saveData.isTimeLimit)
                time = _gameScene._remainingTime;
            else
                time = _gameScene._playingTime;

            _finishScene = new FinishScene(this, _gameLogic, _saveData, _bigFont, _middleFont, _smallFont, _lang, time,
                _buttonSprite_230x30);
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
                _saveData.iDebugModeFromSave = 0;
            }
            _isDebugMode = DetectDebugMode();
            _lang = new Lang(_saveData.langID);
        }
        private bool DetectDebugMode()
        {
            bool isDebugMode = false;
            if (IS_DEBUG_MODE_FROM_CODE || _saveData.iDebugModeFromSave == 1989)
            {
                isDebugMode = true;
            }
            return isDebugMode;
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
