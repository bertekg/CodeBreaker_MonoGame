using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CodeBreaker_MonoGame.Screen
{
    internal class FinishGameScene : IScene
    {
        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _otherFont;
        private readonly string _endGameInfo;
        private readonly Lang _lang;
        private readonly Color _endGameColor;
        private readonly string timeText;
        private readonly string attemptsText;
        private readonly string _correctCodeString;

        public FinishGameScene(GameLogic gameLogic, SaveData saveData, SpriteFont bigFont, SpriteFont middleFont, Lang lang, double time)
        {
            _titleFont = bigFont;
            _otherFont = middleFont;
            _lang = lang;

            if (gameLogic.codeGuessed)
            {
                _endGameInfo = _lang.GetLangText(LangKey.FinishWin); _endGameColor = Color.Green;
            }
            else
            {
                _endGameInfo = _lang.GetLangText(LangKey.FinishLost); _endGameColor = Color.Red;
            }

            if (saveData.isTimeLimit)
            {
                timeText = string.Format(_lang.GetLangText(LangKey.FinishRemainingTime), time);
            }
            else
            {
                timeText = string.Format(_lang.GetLangText(LangKey.FinishPlayingTime), time);
            }

            if (saveData.isAttemptsLimit)
            {
                attemptsText = _lang.GetLangText(LangKey.FinishRemainingAttempts) + (saveData.attemptsLimit - gameLogic.numberOfAttempts).ToString();
            }
            else
            {
                attemptsText = _lang.GetLangText(LangKey.FinishNumberOfAttempts) + gameLogic.numberOfAttempts.ToString();
            }
            _correctCodeString = _lang.GetLangText(LangKey.FinishCorrectCode) + gameLogic.CorrectCodeString();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _endGameInfo, new Vector2(200, 40), _endGameColor);

            spriteBatch.DrawString(_otherFont, timeText, new Vector2(200, 100), Color.Black);

            spriteBatch.DrawString(_otherFont, attemptsText, new Vector2(200, 160), Color.Black);

            spriteBatch.DrawString(_otherFont, _correctCodeString, new Vector2(200, 220), Color.Black);

            spriteBatch.DrawString(_otherFont, _lang.GetLangText(LangKey.FinishPlayAgain), new Vector2(10, 360), Color.Black);
            spriteBatch.DrawString(_otherFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu), new Vector2(10, 400), Color.Black);
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
    }
}
