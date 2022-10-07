﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace CodeBreaker_MonoGame
{
    public enum GameState { Menu, InGame, FinishGame}
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        SpriteFont gameFont;
        SpriteFont debugFont;
        SpriteFont historyFont;
        SpriteFont menuFont;
        Texture2D frameSprite;
        Texture2D menuMarkerSprite;
        SoundEffect clickSoundEffect;
        SoundEffect clickSideSoundEffect;
        SoundEffect menuSoundEffect;
        SoundEffect unlockedSoundEffect;
        SoundEffect succesSoundEffect;
        SoundEffect failSoundEffect;
        SoundEffect startSoundEffect;
        SoundEffect menuSideSoundEffect;
        SoundEffect returnSoundEffect;

        int frameIndex = 0;
        float framePosition = 0.0f;

        bool rightReleased = true;
        bool leftReleased = true;
        bool upRelesed = true;
        bool downRelesed = true;
        bool spaceRelesed = true;
        bool escapeReleased = true;
        bool enterReleased = true;
        bool sReleased = true;

        int codeLength = 4;
        GameLogic gameLogic;

        Vector2 textPlace;

        string debugAns = "Nothing";
        bool isDebugMode = true;

        int maxNumberOfHints = 8;

        GameState gameState = GameState.Menu;

        double playingTime;
        string endGameInfo;
        Color endGameColor;
        double remainingTime;
        double limitTime = 30;
        bool isLimitTime;
        bool isPlaySoundEffect;

        bool isLimitAttempts = false;
        int limitAttempts = 8;

        int menuMarkerIndex = 0;
        float menuMarkerPosition = 0.0f;

        string saveDataPath = System.IO.Path.Combine(System.Environment.CurrentDirectory, "SaveData.xml");

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }        

        protected override void Initialize()
        {
            //StartNewGame();
            GoToMainMenu();
            ReadSaveData();
            _graphics.PreferredBackBufferHeight = 550;
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("spaceFont");
            debugFont = Content.Load<SpriteFont>("debugFont");
            historyFont = Content.Load<SpriteFont>("historyFont");
            menuFont = Content.Load<SpriteFont>("historyFont");
            menuFont = Content.Load<SpriteFont>("menuFont");
            frameSprite = Content.Load<Texture2D>("frame");
            menuMarkerSprite = Content.Load<Texture2D>("menuMarker");
            clickSoundEffect = Content.Load<SoundEffect>("ClickSound");
            clickSideSoundEffect = Content.Load<SoundEffect>("ClickSideSound");
            menuSoundEffect = Content.Load<SoundEffect>("MenuSound");
            unlockedSoundEffect = Content.Load<SoundEffect>("Unlocked");
            succesSoundEffect = Content.Load<SoundEffect>("success");
            failSoundEffect = Content.Load<SoundEffect>("fail");
            startSoundEffect = Content.Load<SoundEffect>("start");
            menuSideSoundEffect = Content.Load<SoundEffect>("menuSide");
            returnSoundEffect = Content.Load<SoundEffect>("return");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && escapeReleased == true)
            {
                if (gameState == GameState.InGame || gameState == GameState.FinishGame)
                {
                    if (isPlaySoundEffect)
                    {
                        returnSoundEffect.Play(1f, 0.5f, 0f);
                    }
                    GoToMainMenu();
                }
                else
                {
                    Exit();
                }
                escapeReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Escape) && escapeReleased == false)
            {
                escapeReleased = true;
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && enterReleased == true)
            {                
                if (gameState == GameState.Menu || gameState == GameState.FinishGame)
                {
                    if (isPlaySoundEffect)
                    {
                        startSoundEffect.Play(1f, 0.5f, 0f);
                    }
                    StartNewGame();
                }
                enterReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Enter) && enterReleased == false)
            {
                enterReleased = true;
            }

            if (keyboardState.IsKeyDown(Keys.Right) && rightReleased == true)
            {
                if (gameState == GameState.InGame)
                {
                    frameIndex++;
                    if (frameIndex > codeLength - 1)
                    {
                        frameIndex = 0;
                    }
                    if (isPlaySoundEffect)
                    {
                        clickSideSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                else if (gameState == GameState.Menu)
                {
                    switch (menuMarkerIndex)
                    {
                        case 0:
                            if (codeLength < 5)
                            {
                                codeLength++;
                            }
                            break;
                        case 1:
                            isLimitAttempts = true;
                            break;
                        case 2:
                            if (limitAttempts < 15)
                            {
                                limitAttempts++;
                            }
                            break;
                        case 3:
                            isLimitTime = true;
                            break;
                        case 4:
                            if (limitTime < 150)
                            {
                                limitTime += 10;
                            }
                            break;
                        case 5:
                            isPlaySoundEffect = true;
                            break;
                        default:
                            break;
                    }
                    if (isPlaySoundEffect)
                    {
                        menuSideSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                rightReleased = false;
            }
            else if(keyboardState.IsKeyUp(Keys.Right) && rightReleased == false)
            {
                rightReleased = true;
            }

            if (keyboardState.IsKeyDown(Keys.Left) && leftReleased == true)
            {
                if (gameState == GameState.InGame)
                {
                    frameIndex--;
                    if (frameIndex < 0)
                    {
                        frameIndex = codeLength - 1;
                    }
                    if (isPlaySoundEffect)
                    {
                        clickSideSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                else if (gameState == GameState.Menu)
                {
                    switch (menuMarkerIndex)
                    {
                        case 0:
                            if (codeLength > 3 )
                            {
                                codeLength--;
                            }
                            break;
                        case 1:
                            isLimitAttempts = false;
                            break;
                        case 2:
                            if (limitAttempts > 3)
                            {
                                limitAttempts--;
                            }
                            break;
                        case 3:
                            isLimitTime = false;
                            break;
                        case 4:
                            if (limitTime > 10)
                            {
                                limitTime -= 10;
                            }
                            break;
                        case 5:
                            isPlaySoundEffect = false;
                            break;
                        default:
                            break;
                    }
                    if (isPlaySoundEffect)
                    {
                        menuSideSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                leftReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Left) && leftReleased == false)
            {
                leftReleased = true;
            }

            framePosition = 35.0f + (frameIndex * 100.0f);

            if (keyboardState.IsKeyDown(Keys.Up) && upRelesed == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[frameIndex]++;
                    if (gameLogic.currentCode[frameIndex] > 9)
                    {
                        gameLogic.currentCode[frameIndex] = 0;
                    }
                    if (isPlaySoundEffect)
                    {
                        clickSoundEffect.Play(1f, 0.5f, 0f);
                    }                    
                }
                else if (gameState == GameState.Menu)
                {
                    menuMarkerIndex--;
                    if (menuMarkerIndex < 0)
                    {
                        menuMarkerIndex = 5;
                    }
                    if ((menuMarkerIndex == 4 && isLimitTime == false) ||
                        (menuMarkerIndex == 2 && isLimitAttempts == false))
                    {
                        menuMarkerIndex--;
                    }
                    if (isPlaySoundEffect)
                    {
                        menuSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                upRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Up) && upRelesed == false)
            {
                upRelesed = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && downRelesed == true)
            {
                if (gameState == GameState.InGame)
                {
                    gameLogic.currentCode[frameIndex]--;
                    if (gameLogic.currentCode[frameIndex] < 0)
                    {
                        gameLogic.currentCode[frameIndex] = 9;
                    }
                    if (isPlaySoundEffect)
                    {
                        clickSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                else if (gameState == GameState.Menu)
                {
                    menuMarkerIndex++;
                    if (menuMarkerIndex > 5)
                    {
                        menuMarkerIndex = 0;
                    }
                    if ((menuMarkerIndex == 4 && isLimitTime == false) ||
                       (menuMarkerIndex == 2 && isLimitAttempts == false))
                    {
                        menuMarkerIndex++;
                    }
                    if (isPlaySoundEffect)
                    {
                        menuSoundEffect.Play(1f, 0.5f, 0f);
                    }
                }
                downRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Down) && downRelesed == false)
            {
                downRelesed = true;
            }

            menuMarkerPosition = 195.0f + (menuMarkerIndex * 50.0f);

            if (keyboardState.IsKeyDown(Keys.Space) && spaceRelesed == true)
            {
                if (gameState == GameState.InGame)
                {
                    bool isCodeCorrect = gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        debugAns = "YES";
                        gameState = GameState.FinishGame;
                        if (isPlaySoundEffect)
                        {
                            succesSoundEffect.Play(1f, 0.5f, 0f);
                        }
                    }
                    else if (((gameLogic.numberOfAttempts >= limitAttempts) && isLimitAttempts))
                    {
                        debugAns = "Attempts";
                        gameState = GameState.FinishGame;
                        if (isPlaySoundEffect)
                        {
                            failSoundEffect.Play(1f, 0.5f, 0f);
                        }
                    }
                    else
                    {
                        debugAns = "no";
                        if (isPlaySoundEffect)
                        {
                            unlockedSoundEffect.Play(1f, 0.5f, 0f);
                        }
                    }
                }                
                spaceRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && spaceRelesed == false)
            {
                spaceRelesed = true;
            }

            if (gameState == GameState.InGame)
            {
                if (isLimitTime)
                {
                    remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (remainingTime <= 0)
                    {
                        remainingTime = 0;
                        gameState = GameState.FinishGame;
                        if (isPlaySoundEffect)
                        {
                            failSoundEffect.Play(1f, 0.5f, 0f);
                        }
                    }
                }
                else
                {
                    playingTime += gameTime.ElapsedGameTime.TotalSeconds;
                }
            }

            if (keyboardState.IsKeyDown(Keys.S) && sReleased == true)
            {
                WriteSaveData();

                sReleased = false;
            }
            else if (keyboardState.IsKeyUp(Keys.S) && escapeReleased == false)
            {
                sReleased = true;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            if (gameState == GameState.InGame)
            {
                for (int i = 0; i < codeLength; i++)
                {
                    _spriteBatch.DrawString(gameFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 110), Color.White);
                }
                _spriteBatch.Draw(frameSprite, new Vector2(framePosition, 110), Color.White);

                if(!isDebugMode)
                {
                    _spriteBatch.DrawString(debugFont, "[Left],[Right] - Move the cursor in the code.", new Vector2(5, 270), Color.White);
                    _spriteBatch.DrawString(debugFont, "[Up],[Down] - Change the value of the indicated digit.", new Vector2(5, 300), Color.White);
                    _spriteBatch.DrawString(debugFont, "[Space] - Check current code.", new Vector2(5, 330), Color.White);
                    _spriteBatch.DrawString(debugFont, "[Esc] - Return to main menu.", new Vector2(5, 360), Color.White);
                    _spriteBatch.DrawString(debugFont, "A single digit can occur only once in the code!", new Vector2(5, 390), Color.White);
                    _spriteBatch.DrawString(debugFont, "Color codes for individual digits:", new Vector2(5, 420), Color.White);
                    _spriteBatch.DrawString(debugFont, "[RED] - Not present in the code.", new Vector2(5, 450), Color.White);
                    _spriteBatch.DrawString(debugFont, "[BLUE] - Appears in the code in a different position.", new Vector2(5, 480), Color.White);
                    _spriteBatch.DrawString(debugFont, "[GREEN] - Is in the correct position.", new Vector2(5, 510), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(debugFont, "Frame index: " + frameIndex.ToString() + ", Frame pos: " + framePosition.ToString(), new Vector2(10, 350), Color.White);
                    _spriteBatch.DrawString(debugFont, "Current code: " + gameLogic.CurrentCodeString(), new Vector2(10, 380), Color.White);
                    _spriteBatch.DrawString(debugFont, "Debug answer: " + debugAns, new Vector2(10, 410), Color.White);
                }

                if (isLimitAttempts)
                {
                    _spriteBatch.DrawString(historyFont, "Remaining Attempts:\n" + (limitAttempts - gameLogic.numberOfAttempts).ToString(), new Vector2(360, 10), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, "Number of Attempts:\n" + gameLogic.numberOfAttempts.ToString(), new Vector2(360, 10), Color.White);
                }
                for (int i = 0; i < gameLogic.guessCodeHistory.Count; i++)
                {
                    for (int j = 0; j < gameLogic.guessCodeHistory[i].Count; j++)
                    {
                        textPlace = new Vector2(600 + (j * 40), (i + 2) * 50);
                        _spriteBatch.DrawString(historyFont, gameLogic.guessCodeHistory[i][j].value.ToString(), textPlace, DecodeColor(gameLogic.guessCodeHistory[i][j].digitState));
                    }
                }

                if (isLimitTime)
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Remaining Time:\n{0:N1}", remainingTime), new Vector2(10, 10), Color.White);
                }
            }
            else if (gameState == GameState.FinishGame)
            {
                endGameInfo = gameLogic.guessedCode ? "You WIN!!!" : "You LOSE!!!";
                endGameColor = gameLogic.guessedCode ? Color.Green : Color.Red;
                _spriteBatch.DrawString(historyFont, endGameInfo, new Vector2(280, 40), endGameColor);
                if (isLimitTime)
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Remaining time {0:N3} seconds", remainingTime), new Vector2(100, 100), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Playing time {0:N3} seconds", playingTime), new Vector2(100, 100), Color.White);
                }
                if (isLimitAttempts)
                {
                    _spriteBatch.DrawString(historyFont, "Remaining Attempts: " + (limitAttempts - gameLogic.numberOfAttempts).ToString(), new Vector2(200, 160), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, "Number of Attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(200, 160), Color.White);
                }
                _spriteBatch.DrawString(historyFont, "Correct code: " + gameLogic.CorrectCodeString(), new Vector2(220, 220), Color.White);
                _spriteBatch.DrawString(historyFont, "Press [Enter] to Start Game again", new Vector2(80, 360), Color.White);
                _spriteBatch.DrawString(historyFont, "Press [Esc] to go back Main Menu", new Vector2(75, 420), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(historyFont, "Code Breaker - MonoGame", new Vector2(120, 40), Color.White);
                _spriteBatch.DrawString(menuFont, "Press [Enter] to Start Game", new Vector2(190, 100), Color.White);
                _spriteBatch.DrawString(menuFont, "Press [Esc] to Exit", new Vector2(270, 140), Color.White);
                _spriteBatch.DrawString(menuFont, "Code length: " + codeLength.ToString(), new Vector2(200, 205), Color.White);
                _spriteBatch.DrawString(menuFont, "Is limit attempts: " + isLimitAttempts.ToString(), new Vector2(200, 255), Color.White);
                _spriteBatch.DrawString(menuFont, "Limit attempts: " + limitAttempts.ToString(), new Vector2(200, 305), isLimitAttempts ? Color.White : Color.Gray);
                _spriteBatch.DrawString(menuFont, "Is limit time: " + isLimitTime.ToString(), new Vector2(200, 355), Color.White);
                _spriteBatch.DrawString(menuFont, "Limit time: " + limitTime.ToString(), new Vector2(200, 405), isLimitTime ? Color.White : Color.Gray);
                _spriteBatch.DrawString(menuFont, "Play sounds effects: " + isPlaySoundEffect.ToString(), new Vector2(200, 455), Color.White);
                _spriteBatch.Draw(menuMarkerSprite, new Vector2(170, menuMarkerPosition), Color.White);
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
            gameLogic = new GameLogic(codeLength, maxNumberOfHints);
            frameIndex = 0;
            playingTime = 0;
            remainingTime = limitTime;
            gameState = GameState.InGame;
        }
        private void GoToMainMenu()
        {
            menuMarkerIndex = 0;
            gameState = GameState.Menu;
        }
        private void ReadSaveData()
        { 
            if(System.IO.File.Exists(saveDataPath))
            {
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));

                System.IO.StreamReader file = new System.IO.StreamReader(saveDataPath);
                SaveData saveData = (SaveData)reader.Deserialize(file);
                file.Close();
                codeLength = saveData.codeLength;
                isLimitAttempts = saveData.isAttempsLimit;
                limitAttempts = saveData.limitAttemps;
                isLimitTime = saveData.isTimeLimit;
                limitTime = saveData.limitTime;
                isPlaySoundEffect = saveData.isPlaySoundEffect;
            }
        }
        protected override void EndRun()
        {
            WriteSaveData();
            base.EndRun();
        }
        private void WriteSaveData()
        {
            SaveData saveData = new SaveData();
            saveData.codeLength = codeLength;
            saveData.isAttempsLimit = isLimitAttempts;
            saveData.limitAttemps = limitAttempts;
            saveData.isTimeLimit = isLimitTime;
            saveData.limitTime = limitTime;
            saveData.isPlaySoundEffect = isPlaySoundEffect;

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(SaveData));


            System.IO.FileStream file = System.IO.File.Create(saveDataPath);
            writer.Serialize(file, saveData);
            file.Close();
        }
    }
}
