using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Texture2D frameSprite;
        int frameIndex = 0;
        float framePosition = 0.0f;

        bool rightReleased = true;
        bool leftReleased = true;
        bool upRelesed = true;
        bool downRelesed = true;
        bool spaceRelesed = true;
        bool escapeReleased = true;
        bool enterReleased = true;

        int codeLength = 3;
        GameLogic gameLogic;

        Vector2 textPlace;

        string debugAns = "Nothing";
        bool isDebugMode = true;

        int maxNumberOfHints = 7;

        //bool inGame = false;
        GameState gameState = GameState.Menu;

        double playingTime;
        string endGameInfo;
        double remainingTime;
        bool isTimeLimit;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }        

        protected override void Initialize()
        {
            StartNewGame();
            gameState = GameState.FinishGame;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            gameFont = Content.Load<SpriteFont>("spaceFont");
            debugFont = Content.Load<SpriteFont>("debugFont");
            historyFont = Content.Load<SpriteFont>("historyFont");
            frameSprite = Content.Load<Texture2D>("frame");
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape) && escapeReleased == true)
            {
                if (gameState == GameState.InGame || gameState == GameState.FinishGame)
                {
                    gameState = GameState.Menu;
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
                }                
                downRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Down) && downRelesed == false)
            {
                downRelesed = true;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && spaceRelesed == true)
            {
                if (gameState == GameState.InGame)
                {
                    bool isCodeCorrect = gameLogic.TryCode();
                    if (isCodeCorrect)
                    {
                        debugAns = "YES";
                        gameState = GameState.FinishGame;
                    }
                    else
                    {
                        debugAns = "no";
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

                _spriteBatch.DrawString(historyFont, "Number of Attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(320, 40), Color.White);
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
                _spriteBatch.DrawString(historyFont, endGameInfo, new Vector2(280, 40), Color.White);
                if (isTimeLimit)
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Remainig time {0:N3} secounds", remainingTime), new Vector2(100, 100), Color.White);
                }
                else
                {
                    _spriteBatch.DrawString(historyFont, string.Format("Playing time {0:N3} secounds", playingTime), new Vector2(100, 100), Color.White);
                }
                _spriteBatch.DrawString(historyFont, "Number of Attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(200, 160), Color.White);
                _spriteBatch.DrawString(historyFont, "Correct code: " + gameLogic.CorrectCodeString(), new Vector2(220, 220), Color.White);
                _spriteBatch.DrawString(historyFont, "Press [Enter] to Start Game again", new Vector2(80, 360), Color.White);
                _spriteBatch.DrawString(historyFont, "Press [Esc] to go back Main Menu", new Vector2(75, 420), Color.White);
            }
            else
            {
                _spriteBatch.DrawString(historyFont, "Code Breaker - MonoGame", new Vector2(120, 40), Color.White);
                _spriteBatch.DrawString(historyFont, "Press [Enter] to Start Game", new Vector2(120, 100), Color.White);
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
            remainingTime = 10;
            isTimeLimit = false;
            gameState = GameState.InGame;
        }
    }
}
