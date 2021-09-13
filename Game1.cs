using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CodeBreaker_MonoGame
{
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

        int codeLength = 4;
        GameLogic gameLogic;

        Vector2 textPlace;

        string debugAns = "Nothing";
        bool isDebugMode = true;

        int maxNumberOfHints = 7;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameLogic = new GameLogic(codeLength, maxNumberOfHints);

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || keyboardState.IsKeyDown(Keys.Escape))
                Exit();

            if(keyboardState.IsKeyDown(Keys.Right) && rightReleased == true)
            {
                frameIndex++;
                if (frameIndex > codeLength - 1)
                {
                    frameIndex = 0;
                }
                rightReleased = false;
            }
            else if(keyboardState.IsKeyUp(Keys.Right) && rightReleased == false)
            {
                rightReleased = true;
            }

            if (keyboardState.IsKeyDown(Keys.Left) && leftReleased == true)
            {
                frameIndex--;
                if (frameIndex < 0)
                {
                    frameIndex = codeLength - 1;
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
                gameLogic.currentCode[frameIndex]++;
                if (gameLogic.currentCode[frameIndex] > 9)
                {
                    gameLogic.currentCode[frameIndex] = 0;
                }
                upRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Up) && upRelesed == false)
            {
                upRelesed = true;
            }

            if (keyboardState.IsKeyDown(Keys.Down) && downRelesed == true)
            {
                gameLogic.currentCode[frameIndex]--;
                if (gameLogic.currentCode[frameIndex] < 0)
                {
                    gameLogic.currentCode[frameIndex] = 9;
                }
                downRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Down) && downRelesed == false)
            {
                downRelesed = true;
            }

            if (keyboardState.IsKeyDown(Keys.Space) && spaceRelesed == true)
            {
                bool isCodeCorrect = gameLogic.TryCode();
                if (isCodeCorrect)
                {
                    debugAns = "YES";
                }
                else
                {
                    debugAns = "no";
                }
                spaceRelesed = false;
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && spaceRelesed == false)
            {
                spaceRelesed = true;
            }            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (int i = 0; i < codeLength; i++)
            {
                _spriteBatch.DrawString(gameFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 100), Color.White);
            }
            _spriteBatch.Draw(frameSprite, new Vector2(framePosition, 100), Color.White);

            if (isDebugMode)
            {
                _spriteBatch.DrawString(debugFont, "Frame index: " + frameIndex.ToString() + ", Frame pos: " + framePosition.ToString(), new Vector2(3, 3), Color.White);
                _spriteBatch.DrawString(debugFont, "Current code: " + gameLogic.CurrentCodeString(), new Vector2(3, 25), Color.White);
                _spriteBatch.DrawString(debugFont, "Debug answere: " + debugAns, new Vector2(3, 50), Color.White);
            }            

            _spriteBatch.DrawString(historyFont, "Number of attempts: " + gameLogic.numberOfAttempts.ToString(), new Vector2(320, 40), Color.White);
            for (int i = 0; i < gameLogic.guessCodeHistory.Count; i++)
            {
                for (int j = 0; j < gameLogic.guessCodeHistory[i].Count; j++)
                {
                    textPlace = new Vector2(600 + (j * 40), (i + 2) * 50);
                    _spriteBatch.DrawString(historyFont, gameLogic.guessCodeHistory[i][j].value.ToString(), textPlace, DecodeColor(gameLogic.guessCodeHistory[i][j].digitState));
                }
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
    }
}
