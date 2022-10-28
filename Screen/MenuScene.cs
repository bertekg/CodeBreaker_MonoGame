using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CodeBreaker_MonoGame.Screen
{
    internal class MenuScene : IScene
    {
        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _navigationScene;
        private readonly SpriteFont _optionFont;
        private readonly SpriteFont _creditsFont;
        private readonly Lang _lang;

        private readonly Texture2D _iconGame;
        private Vector2 _iconLocation;

        const int MENU_MARKER_START_X = 225, MENU_MARKER_START_Y = 330, MENU_MARKER_STEP_Y = 40;

        private SaveData _saveData;
        private readonly string _versionGame;

        private Marker _menuMarker;

        public MenuScene(SpriteFont titleFont, SpriteFont navigationScene, SpriteFont optionFont, SpriteFont creditsFont,
            Lang lang, Texture2D iconGame, string versionGame, Texture2D markerSprite, SaveData saveData)
        {
            _titleFont = titleFont;
            _navigationScene = navigationScene;
            _optionFont = optionFont;
            _creditsFont = creditsFont;
            _lang = lang;
            _iconGame = iconGame;
            _iconLocation = new Vector2(60, 27);
            _versionGame = versionGame;
            _menuMarker = new Marker(markerSprite, 4, new Vector2(MENU_MARKER_START_X - 15, MENU_MARKER_START_Y - 5),
                new Vector2(210, MENU_MARKER_START_Y + (4 * MENU_MARKER_STEP_Y) - 5));
            _saveData = saveData;
            MoveMarker(0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameTitle), new Vector2(180, 50), Color.Black);
            spriteBatch.Draw(_iconGame, _iconLocation, Color.White);

            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.StartGameKey), new Vector2(120, 140), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameInstuctionKey), new Vector2(120, 170), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.GameOptionKey), new Vector2(120, 200), Color.Black);
            spriteBatch.DrawString(_navigationScene, _lang.GetLangText(LangKey.ExitGameKey), new Vector2(120, 230), Color.Black);

            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.GameModifiers), GetMenuMarkerPosition(-1), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.CodeLength) + _saveData.codeLength.ToString(), GetMenuMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.IsLimitedAttempts) + _lang.GetBoolInLang(_saveData.isAttemptsLimit), GetMenuMarkerPosition(1), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.NumberAttempts) + _saveData.attemptsLimit.ToString(), GetMenuMarkerPosition(2), _saveData.isAttemptsLimit ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.IsTimeLimitation) + _lang.GetBoolInLang(_saveData.isTimeLimit), GetMenuMarkerPosition(3), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.TimeLimit) + _saveData.timeLimit.ToString(), GetMenuMarkerPosition(4), _saveData.isTimeLimit ? Color.Black : Color.Gray);

            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.CreditsStart) + "Bartłomiej Grywalski", new Vector2(20, 530), Color.Black);
            spriteBatch.DrawString(_creditsFont, _lang.GetLangText(LangKey.VersionInfo) + _versionGame, new Vector2(600, 530), Color.Black);

            _menuMarker.Draw(spriteBatch);
        }

        public void Update()
        {
            throw new NotImplementedException();
        }
        public void MoveMarker(int index)
        {
            _menuMarker.MoveMarker(index);
            _menuMarker.Update();
        }
        private Vector2 GetMenuMarkerPosition(int row)
        {
            return new Vector2(MENU_MARKER_START_X, MENU_MARKER_START_Y + (row * MENU_MARKER_STEP_Y));
        }
    }
}
