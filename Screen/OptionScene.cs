using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Reflection;

namespace CodeBreaker_MonoGame.Screen
{
    internal class OptionScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont;
        private readonly SpriteFont _optionFont;
        private readonly SpriteFont _navigationFont;
        private readonly Lang _lang;
        private Marker _optionMarker;
        private bool _isSounding;
        private string _musicVolume, _soundsVolume;
        private bool _keyRightUp, _keyLeftUp, _keyUpUp, _keyDownUp;
        private int _optionMarkerIndex;
        private readonly SoundEffect _menuSideSoundEffect, _menuUpDownSoundEffect;

        const int OPTION_MARKER_START_X = 225, OPTION_MARKER_START_Y = 150, PTION_MARKER_STEP_Y = 40;
        public OptionScene(Game1 game1, Texture2D markerSprite, SpriteFont titleFont, SpriteFont optionFont,
            SpriteFont navigationFont, Lang lang, MusicAndSounds musicAndSounds, 
            SoundEffect menuSideSoundEffect, SoundEffect menuUpDownSoundEffect)
        {
            _game1 = game1;
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
            _menuSideSoundEffect = menuSideSoundEffect;
            _menuUpDownSoundEffect = menuUpDownSoundEffect;
            _optionMarkerIndex = 0;
            MoveMarker(_optionMarkerIndex);
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
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                _game1.GoToMainMenu(true);

            if ((keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D) || gamePadState.DPad.Right == ButtonState.Pressed)
                && _keyRightUp == true)
            {
                switch (_optionMarkerIndex)
                {
                    case 0:
                        _game1.musicAndSounds.EditIsSounding(true);
                        SetIsSounding(_game1.musicAndSounds.GetIsSounding());
                        break;
                    case 1:
                        _game1.musicAndSounds.IncMusicVolume();
                        SetMusicVolume(_game1.musicAndSounds.GetMusicVolumePercentString());
                        break;
                    case 2:
                        _game1.musicAndSounds.IncSoundsVolume();
                        SetSoundsVolume(_game1.musicAndSounds.GetSoundsVolumePercentString());
                        break;
                    case 3:
                        _lang.IncSelection();
                        break;
                    default:
                        break;
                }
                _game1.musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);
                _keyRightUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Right) && keyboardState.IsKeyUp(Keys.D) && gamePadState.DPad.Right == ButtonState.Released)
                && _keyRightUp == false)
                _keyRightUp = true;

            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
                && _keyLeftUp == true)
            {
                switch (_optionMarkerIndex)
                {
                    case 0:
                        _game1.musicAndSounds.EditIsSounding(false);
                        SetIsSounding(_game1.musicAndSounds.GetIsSounding());
                        break;
                    case 1:
                        _game1.musicAndSounds.DecMusicVolume();
                        SetMusicVolume(_game1.musicAndSounds.GetMusicVolumePercentString());
                        break;
                    case 2:
                        _game1.musicAndSounds.DecSoundsVolume();
                        SetSoundsVolume(_game1.musicAndSounds.GetSoundsVolumePercentString());
                        break;
                    case 3:
                        _lang.DecSelection();
                        break;
                    default:
                        break;
                }
                _game1.musicAndSounds.PlaySoundEffect(_menuSideSoundEffect);
                _keyLeftUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A) && gamePadState.DPad.Left == ButtonState.Released)
                && _keyLeftUp == false)
                _keyLeftUp = true;

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                && _keyUpUp == true)
            {
                _optionMarkerIndex--;
                if (_optionMarkerIndex < 0)
                {
                    _optionMarkerIndex = 3;
                }
                if (_optionMarkerIndex == 2 && _game1.musicAndSounds.GetIsSounding() == false)
                {
                    _optionMarkerIndex -= 2;
                }
                MoveMarker(_optionMarkerIndex);
                _game1.musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyUpUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W) && gamePadState.DPad.Up == ButtonState.Released)
                && _keyUpUp == false)
                _keyUpUp = true;

            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
                && _keyDownUp == true)
            {
                _optionMarkerIndex++;
                if (_optionMarkerIndex == 1 && _game1.musicAndSounds.GetIsSounding() == false)
                {
                    _optionMarkerIndex += 2;
                }
                if (_optionMarkerIndex > 3)
                {
                    _optionMarkerIndex = 0;
                }
                MoveMarker(_optionMarkerIndex);
                _game1.musicAndSounds.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyDownUp = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
                && _keyDownUp == false)
                _keyDownUp = true;
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
