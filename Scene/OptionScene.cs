using CodeBreaker_MonoGame.Class;
using CodeBreaker_MonoGame.Control;
using CodeBreaker_MonoGame.Interface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace CodeBreaker_MonoGame.Scene
{
    internal class OptionScene : IScene
    {
        private readonly Game1 _game1;
        private readonly SpriteFont _titleFont, _optionFont, _navigationFont;
        private readonly Lang _lang;
        private Marker _optionMarker;
        private bool _isSounding;
        private string _musicVolume, _soundsVolume;
        KeyboardState keyboardState, _previousKS;
        GamePadState gamePadState, _previousGPS;
        private bool _keyLeftReleased, _keyUpReleased, _keyDownReleased;
        private int _optionMarkerIndex;
        private readonly SoundEffect _menuSideSoundEffect, _menuUpDownSoundEffect;

        private Vector2 _locTitle, _locBackToolTip;
        private List<Component> _gameComponents;
        private readonly Texture2D _buttonNavigation, _buttonOption;

        private Button _goMenuButton, _isSoundingTrueButton, _isSoundingFalseButton, _musicVolumeInc, _musicVolumeDec,
            _soundVolumeInc, _soundVolumeDec, _langSwitchLeftButton, _langSwitchRightButton;
        private readonly Vector2 _offsetButtonOptionRight, _offsetButtonOptionLeft;

        const int OPTION_MARKER_START_X = 225, OPTION_MARKER_START_Y = 150, PTION_MARKER_STEP_Y = 40;
        public OptionScene(Game1 game1, Texture2D markerSprite, SpriteFont titleFont, SpriteFont optionFont,
            SpriteFont navigationFont, Lang lang, MusicAndSounds musicAndSounds, 
            SoundEffect menuSideSoundEffect, SoundEffect menuUpDownSoundEffect, Texture2D buttonNavigation,
            Texture2D buttonOption)
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

            _locTitle = new Vector2((Game1.ScreenWidth / 2) -
                (_titleFont.MeasureString(_lang.GetLangText(LangKey.GameOption)).X / 2), 30);

            _offsetButtonOptionRight = new Vector2(-55, 0);
            _offsetButtonOptionLeft = new Vector2(-90, 0);
            _buttonNavigation = buttonNavigation;
            _buttonOption = buttonOption;
            InitializeButtons();
        }
        private void InitializeButtons()
        {
            _goMenuButton = new Button(_buttonNavigation, _navigationFont)
            {
                Position = new Vector2((Game1.ScreenWidth / 2) - (_buttonNavigation.Width / 2), 470),
                Text = _lang.GetLangText(LangKey.Back)
            };
            _goMenuButton.Click += GoMenuButton_Click;
            _locBackToolTip = new Vector2(((Game1.ScreenWidth / 2) -
                (_navigationFont.MeasureString(_lang.GetLangText(LangKey.GoBackMenu)).X / 2)), 500);

            _isSoundingTrueButton = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(0) + _offsetButtonOptionRight,
                Text = _lang.GetLangText(LangKey.TrueSingleLetter)
            };
            _isSoundingTrueButton.Click += IsSoundingTrue_Click;

            _isSoundingFalseButton = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(0) + _offsetButtonOptionLeft,
                Text = _lang.GetLangText(LangKey.FalseSingleLetter)
            };
            _isSoundingFalseButton.Click += IsSoundingFalse_Click;

            bool isNoSounding = !_game1.musicAndSounds.GetIsSounding();
            _musicVolumeInc = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(1) + _offsetButtonOptionRight,
                Text = "+",
                IsDisable = isNoSounding
            };
            _musicVolumeInc.Click += MusicVolumeInc_Click;

            _musicVolumeDec = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(1) + _offsetButtonOptionLeft,
                Text = "-",
                IsDisable = isNoSounding
            };
            _musicVolumeDec.Click += MusicVolumeDec_Click;

            _soundVolumeInc = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(2) + _offsetButtonOptionRight,
                Text = "+",
                IsDisable = isNoSounding
            };
            _soundVolumeInc.Click += SoundVolumeInc_Click;

            _soundVolumeDec = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(2) + _offsetButtonOptionLeft,
                Text = "-",
                IsDisable = isNoSounding
            };
            _soundVolumeDec.Click += SoundVolumeDec_Click;

            _langSwitchRightButton = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(3) + _offsetButtonOptionRight,
                Text = ">"
            };
            _langSwitchRightButton.Click += LangSwitchRight_Click;

            _langSwitchLeftButton = new Button(_buttonOption, _optionFont)
            {
                Position = GetOptionMarkerPosition(3) + _offsetButtonOptionLeft,
                Text = "<"
            };
            _langSwitchLeftButton.Click += LangSwitchLeft_Click;

            _gameComponents = new List<Component>()
            {
                _goMenuButton, _isSoundingTrueButton, _isSoundingFalseButton, _musicVolumeInc, _musicVolumeDec,
                _soundVolumeInc, _soundVolumeDec, _langSwitchRightButton, _langSwitchLeftButton
            };
        }
        private void GoMenuButton_Click(object sender, EventArgs e)
        {
            GoToMainMenu();
        }
        private void IsSoundingTrue_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 0;
            MoveMarker(_optionMarkerIndex);
            IsSoundingTrue();
        }
        private void IsSoundingFalse_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 0;
            MoveMarker(_optionMarkerIndex);
            IsSoundingFalse();
        }
        private void MusicVolumeInc_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 1;
            MoveMarker(_optionMarkerIndex);
            MusicVolumeInc();
        }
        private void MusicVolumeDec_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 1;
            MoveMarker(_optionMarkerIndex);
            MusicVolumeDec();
        }
        private void SoundVolumeInc_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 2;
            MoveMarker(_optionMarkerIndex);
            SoundVolumeInc();
        }
        private void SoundVolumeDec_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 2;
            MoveMarker(_optionMarkerIndex);
            SoundVolumeDec();
        }
        private void LangSwitchRight_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 3;
            MoveMarker(_optionMarkerIndex);
            LangSwitchRight();
        }
        private void LangSwitchLeft_Click(object sender, EventArgs e)
        {
            _optionMarkerIndex = 3;
            MoveMarker(_optionMarkerIndex);
            LangSwitchLeft();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_titleFont, _lang.GetLangText(LangKey.GameOption), _locTitle, Color.Black);

            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.PlayingSound) + _lang.GetBoolInLang(_isSounding),
                GetOptionMarkerPosition(0), Color.Black);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.MusicVolume) + _musicVolume,
                GetOptionMarkerPosition(1), _isSounding ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.SoundsVolume) + _soundsVolume,
                GetOptionMarkerPosition(2), _isSounding ? Color.Black : Color.Gray);
            spriteBatch.DrawString(_optionFont, _lang.GetLangText(LangKey.LanguageSelected),
                GetOptionMarkerPosition(3), Color.Black);

            _optionMarker.Draw(spriteBatch);

            foreach (var component in _gameComponents)
                component.Draw(spriteBatch);

            spriteBatch.DrawString(_navigationFont, _lang.GetLangText(LangKey.GoBackMenu), _locBackToolTip, Color.Black);
        }
        public void Update(double deltaTime)
        {
            _previousKS = keyboardState;
            keyboardState = Keyboard.GetState();
            _previousGPS = gamePadState;
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Escape) || gamePadState.Buttons.Back == ButtonState.Pressed)
                GoToMainMenu();

            if ( (keyboardState.IsKeyDown(Keys.Right) && _previousKS.IsKeyUp(Keys.Right)) ||
                (keyboardState.IsKeyDown(Keys.D) && _previousKS.IsKeyUp(Keys.D)) ||
                (gamePadState.IsButtonDown(Buttons.DPadRight) && _previousGPS.IsButtonUp(Buttons.DPadRight) ) )
            {
                switch (_optionMarkerIndex)
                {
                    case 0:
                        IsSoundingTrue();
                        break;
                    case 1:
                        MusicVolumeInc();
                        break;
                    case 2:
                        SoundVolumeInc();
                        break;
                    case 3:
                        LangSwitchRight();
                        break;
                    default:
                        break;
                }
            }

            if ((keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A) || gamePadState.DPad.Left == ButtonState.Pressed)
                && _keyLeftReleased == true)
            {
                switch (_optionMarkerIndex)
                {
                    case 0:
                        IsSoundingFalse();
                        break;
                    case 1:
                        MusicVolumeDec();
                        break;
                    case 2:
                        SoundVolumeDec();
                        break;
                    case 3:
                        LangSwitchLeft();
                        break;
                    default:
                        break;
                }
                
                _keyLeftReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Left) && keyboardState.IsKeyUp(Keys.A) && gamePadState.DPad.Left == ButtonState.Released)
                && _keyLeftReleased == false)
                _keyLeftReleased = true;

            if ((keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W) || gamePadState.DPad.Up == ButtonState.Pressed)
                && _keyUpReleased == true)
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
                _game1.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyUpReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Up) && keyboardState.IsKeyUp(Keys.W) && gamePadState.DPad.Up == ButtonState.Released)
                && _keyUpReleased == false)
                _keyUpReleased = true;

            if ((keyboardState.IsKeyDown(Keys.Down) || keyboardState.IsKeyDown(Keys.S) || gamePadState.DPad.Down == ButtonState.Pressed)
                && _keyDownReleased == true)
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
                _game1.PlaySoundEffect(_menuUpDownSoundEffect);
                _keyDownReleased = false;
            }
            else if ((keyboardState.IsKeyUp(Keys.Down) && keyboardState.IsKeyUp(Keys.S) && gamePadState.DPad.Down == ButtonState.Released)
                && _keyDownReleased == false)
                _keyDownReleased = true;

            foreach (var component in _gameComponents)
                component.Update();
        }

        private void LangSwitchLeft()
        {
            _lang.DecSelection();
            UpdateLangAfterChange();
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void LangSwitchRight()
        {
            _lang.IncSelection();
            UpdateLangAfterChange();
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void SoundVolumeDec()
        {
            _game1.musicAndSounds.DecSoundsVolume();
            SetSoundsVolume(_game1.musicAndSounds.GetSoundsVolumePercentString());
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void MusicVolumeDec()
        {
            _game1.musicAndSounds.DecMusicVolume();
            SetMusicVolume(_game1.musicAndSounds.GetMusicVolumePercentString());
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void SoundVolumeInc()
        {
            _game1.musicAndSounds.IncSoundsVolume();
            SetSoundsVolume(_game1.musicAndSounds.GetSoundsVolumePercentString());
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void MusicVolumeInc()
        {
            _game1.musicAndSounds.IncMusicVolume();
            SetMusicVolume(_game1.musicAndSounds.GetMusicVolumePercentString());
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void IsSoundingFalse()
        {
            _game1.musicAndSounds.EditIsSounding(false);
            SetIsSounding(_game1.musicAndSounds.GetIsSounding());
            _musicVolumeInc.IsDisable = true;
            _musicVolumeDec.IsDisable = true;
            _soundVolumeInc.IsDisable = true;
            _soundVolumeDec.IsDisable = true;
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void IsSoundingTrue()
        {
            _game1.musicAndSounds.EditIsSounding(true);
            SetIsSounding(_game1.musicAndSounds.GetIsSounding());
            _musicVolumeInc.IsDisable = false;
            _musicVolumeDec.IsDisable = false;
            _soundVolumeInc.IsDisable = false;
            _soundVolumeDec.IsDisable = false;
            _game1.PlaySoundEffect(_menuSideSoundEffect);
        }

        private void UpdateLangAfterChange()
        {
            _locTitle = new Vector2((Game1.ScreenWidth / 2) -
                (_titleFont.MeasureString(_lang.GetLangText(LangKey.GameOption)).X / 2), 30);
            _locBackToolTip = new Vector2(((Game1.ScreenWidth / 2) -
               (_navigationFont.MeasureString(_lang.GetLangText(LangKey.GoBackMenu)).X / 2)), 500);
            _goMenuButton.Text = _lang.GetLangText(LangKey.Back);
            _isSoundingFalseButton.Text = _lang.GetLangText(LangKey.FalseSingleLetter);
            _isSoundingTrueButton.Text = _lang.GetLangText(LangKey.TrueSingleLetter);
        }

        private void GoToMainMenu()
        {
            _game1.GoToMainMenu(true);
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
