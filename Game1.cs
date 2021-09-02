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

        int codeLength = 5;
        GameLogic gameLogic;

        Vector2 textPlace;

        string debugAns = "Nothing";

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gameLogic = new GameLogic(codeLength);

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
                _spriteBatch.DrawString(gameFont, gameLogic.currentCode[i].ToString(), new Vector2(50 + (100 * i), 80), Color.White);
            }
            _spriteBatch.Draw(frameSprite, new Vector2(framePosition, 80), Color.White);
            _spriteBatch.DrawString(debugFont, "Frame idex: " + frameIndex.ToString() + ", Frame pos: " + framePosition.ToString(), new Vector2(3, 3), Color.White);            
            _spriteBatch.DrawString(debugFont, "Current code: " + gameLogic.CurrentCodeString(), new Vector2(3, 25), Color.White);
            _spriteBatch.DrawString(debugFont, "Debug answere: " + debugAns, new Vector2(3, 50), Color.White);
            for (int i = 0; i < gameLogic.rowCount; i++)
            {
                _spriteBatch.DrawString(historyFont, (i + 1).ToString() +":", new Vector2(550, 50 + (i * 50)), Color.White);
            }
            foreach (SingleDigit singleDigit in gameLogic.guessCodeHistory)
            {
                textPlace = new Vector2(600 + (singleDigit.column * 40),50 + (singleDigit.row * 50));
                _spriteBatch.DrawString(historyFont, singleDigit.value.ToString(), textPlace, DecodeColor(singleDigit.digitState));
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
