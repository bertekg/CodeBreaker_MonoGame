using Microsoft.Xna.Framework;
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
        SoundEffect sitllLockedSoundEffect;

        int frameIndex = 0;
        float framePosition = 0.0f;

        bool rightReleased = true;
        bool leftReleased = true;
        bool upRelesed = true;
        bool downRelesed = true;
        bool spaceRelesed = true;
        bool escapeReleased = true;
        bool enterReleased = true;

        int codeLength = 4;
        GameLogic gameLogic;

        Vector2 textPlace;

        string debugAns = "Nothing";
        bool isDebugMode = true;

        int maxNumberOfHints = 7;

        //bool inGame = false;
        GameState gameState = GameState.Menu;

        double playingTime;
        string endGameInfo;
        Color endGameColor;
        double remainingTime;
        double limitTime = 30;
        bool isTimeLimit;

        bool isAttempsLimit;
        int limitAttemps = 7;

        int menuMarkerIndex = 0;
        float menuMarkerPosition = 0.0f;

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
            sitllLockedSoundEffect = Content.Load<SoundEffect>("StillLocked");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && escapeReleased == true)
            {
                if (gameState == GameState.InGame || gameState == GameState.FinishGame)
                {
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
                    clickSideSoundEffect.Play(1f, 0.5f, 0f);
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
                            isAttempsLimit = true;
                            break;
                        case 2:
                            if (limitAttemps < 15)
                            {
                                limitAttemps++;
                            }
                            break;
                        case 3:
                            isTimeLimit = true;
                            break;
                        case 4:
                            if (limitTime < 150)
                            {
                                limitTime += 10;
                            }
                            break;
                        default:
                            break;
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
                    clickSideSoundEffect.Play(1f, 0.5f, 0f);
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
                            isAttempsLimit = false;
                            break;
                        case 2:
                            if (limitAttemps > 3)
                            {
                                limitAttemps--;
                            }
                            break;
                        case 3:
                            isTimeLimit = false;
                            break;
                        case 4:
                            if (limitTime > 10)
                            {
                                limitTime -= 10;
                            }
                            break;
                        default:
                            break;
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
                    clickSoundEffect.Play(1f, 0.5f, 0f);
                }
                else if (gameState == GameState.Menu)
                {
                    menuMarkerIndex--;
                    if (menuMarkerIndex < 0)
                    {
                        menuMarkerIndex = 4;
                    }
                    menuSoundEffect.Play(1f, 0.5f, 0f);
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
                    clickSoundEffect.Play(1f, 0.5f, 0f);
                }
                else if (gameState == GameState.Menu)
                {
                    menuMarkerIndex++;
                    if (menuMarkerIndex > 4)
                    {
                        menuMarkerIndex = 0;
                    }
                    menuSoundEffect.Play(1f, 0.5f, 0f);
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
                    if (isCodeCorrect || ((gameLogic.numberOfAttempts >= limitAttemps) && isAttempsLimit))
                    {
                        unlockedSoundEffect.Play(1f, 0.5f, 0f);
                        debugAns = "YES";
                        gameState = GameState.FinishGame;
                    }
                    else
                    {
                        debugAns = "no";
                        sitllLockedSoundEffect.Play(1f, 0.5f, 0f);
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
                if (isTimeLimit)
                {
                    remainingTime -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (remainingTime <= 0)
                    {
                        remainingTime = 0;
                        gameState = GameState.FinishGame;
                    }
                }
                else
                {
                    playingTime += gameTime.ElapsedGameTime.TotalSeconds;
                }
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
                    _spriteBatch.DrawString(gameFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 100), Color.White);
                }
                _spriteBatch.Draw(frameSprite, new Vector2(framePosition, 100), Color.White);

                if (isDebugMode)
                {
                    _spriteBatch.DrawString(debugFont, "Frame index: " + frameIndex.ToString() + ", Frame pos: " + framePosition.ToString(), new Vector2(3, 350), Color.White);
                    _spriteBatch.DrawString(debugFont, "Current code: " + gameLogic.CurrentCodeString(), new Vector2(3, 380), Color.White);
                    _spriteBatch.DrawString(debugFont, "Debug answere: " + debugAns, new Vector2(3, 410), Color.White);
                }

                if (isAttempsLimit)
                {
                    _spriteBatch.DrawString(historyFont, "Remainig Attempts: " + (limitAttemps - gameLogic.numberOfAttempts).ToString(), new Vector2(320, 40), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, "Number of Attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(320, 40), Color.White);
                }
                for (int i = 0; i < gameLogic.guessCodeHistory.Count; i++)
                {
                    for (int j = 0; j < gameLogic.guessCodeHistory[i].Count; j++)
                    {
                        textPlace = new Vector2(600 + (j * 40), (i + 2) * 50);
                        _spriteBatch.DrawString(historyFont, gameLogic.guessCodeHistory[i][j].value.ToString(), textPlace, DecodeColor(gameLogic.guessCodeHistory[i][j].digitState));
                    }
                }

                if (isTimeLimit)
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Remainig Time: {0:N1}", remainingTime), new Vector2(50, 280), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Playing Time: {0:N1}", playingTime), new Vector2(50, 280), Color.White);
                }
            }
            else if (gameState == GameState.FinishGame)
            {
                endGameInfo = gameLogic.guessedCode ? "You WIN!!!" : "You LOSE!!!";
                endGameColor = gameLogic.guessedCode ? Color.Green : Color.Red;
                _spriteBatch.DrawString(historyFont, endGameInfo, new Vector2(280, 40), endGameColor);
                if (isTimeLimit)
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Remainig time {0:N3} secounds", remainingTime), new Vector2(100, 100), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Playing time {0:N3} secounds", playingTime), new Vector2(100, 100), Color.White);
                }
                if (isAttempsLimit)
                {
                    _spriteBatch.DrawString(historyFont, "Remainig Attempts: " + (limitAttemps - gameLogic.numberOfAttempts).ToString(), new Vector2(200, 160), Color.White);
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
                _spriteBatch.DrawString(menuFont, codeLength.ToString() + ": Code length", new Vector2(180, 205), Color.White);
                _spriteBatch.DrawString(menuFont, isAttempsLimit.ToString() + ": Is limit attemps", new Vector2(180, 255), Color.White);
                _spriteBatch.DrawString(menuFont, limitAttemps.ToString() + ": Limit attemps", new Vector2(180, 305), Color.White);
                _spriteBatch.DrawString(menuFont, isTimeLimit.ToString() + ": Is limit time", new Vector2(180, 355), Color.White);
                _spriteBatch.DrawString(menuFont, limitTime.ToString() + ": Limit time", new Vector2(180, 405), Color.White);
                _spriteBatch.Draw(menuMarkerSprite, new Vector2(150, menuMarkerPosition), Color.White);
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
    }
}
