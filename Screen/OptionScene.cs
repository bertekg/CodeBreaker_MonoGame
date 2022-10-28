using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;

namespace CodeBreaker_MonoGame.Screen
{
    internal class OptionScene : IScene
    {
        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _optionFont;
        private readonly SpriteFont _navigationFont;
        private readonly Lang _lang;
        private Marker _optionMarker;
        private bool _isSounding;
        private string _musicVolume, _soundsVolume;

        const int OPTION_MARKER_START_X = 225, OPTION_MARKER_START_Y = 150, PTION_MARKER_STEP_Y = 40;
        public OptionScene(Texture2D markerSprite, SpriteFont titleFont, SpriteFont optionFont,
            SpriteFont navigationFont, Lang lang, MusicAndSounds musicAndSounds)
        {
            _optionMarker = new Marker(markerSprite, 3,
                new Vector2(210, OPTION_MARKER_START_Y - 5),
                new Vector2(210, OPTION_MARKER_START_Y + (3 * PTION_MARKER_STEP_Y) - 5));
            _titleFont = titleFont;
            _optionFont = optionFont;
            _navigationFont = navigationFont;
            _lang = lang;
            _isSounding = musicAndSounds.GetIsSounding();
            _musicVolume = musicAndSounds.GetMusicVolumePercentString();
            _soundsVolume = musicAndSounds.GetSoundsVolumePercentString();
            MoveMarker(0);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameOption), new Vector2(300, 50), Color.Black);

            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.PlayingSound) + _lang.GetBoolInLang(_isSounding),
                GetOptionMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.MusicVolume) + _musicVolume,
                GetOptionMarkerPosition(1), _isSounding ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.SoundsVolume) + _soundsVolume,
                GetOptionMarkerPosition(2), _isSounding ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.LanguageSelected),
                GetOptionMarkerPosition(3), Color.Black);

            _optionMarker.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.InstrucitonAndFinishGoBackMenu),
                new Vector2(10, 500), Color.Black);
        }
        public void SetIsSounding(bool isSounding)
        {
            _isSounding = isSounding;
        }
        public void SetMusicVolume(string musicVolume)
        {
            _musicVolume = musicVolume;
        }
        public void SetSoundsVolume(string soundsVolume)
        {
            _soundsVolume = soundsVolume;
        }
        public void Update()
        {
            throw new NotImplementedException();
        }
        public void MoveMarker(int index)
        {
            _optionMarker.MoveMarker(index);
            _optionMarker.Update();
        }
        private Vector2 GetOptionMarkerPosition(int row)
        {
            return new Vector2(OPTION_MARKER_START_X, OPTION_MARKER_START_Y + (row * PTION_MARKER_STEP_Y));
        }
    }
}
