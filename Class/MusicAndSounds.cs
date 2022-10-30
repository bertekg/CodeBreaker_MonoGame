using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace CodeBreaker_MonoGame.Class
{
    public class MusicAndSounds
    {
        private Song _music;
        private bool _isSounding;
        private sbyte _musicVolumePercent;
        private sbyte _soundsVolumePercent;

        const sbyte VOLUME_STEP = 10;
        public MusicAndSounds(Song music, SaveData saveData)
        {
            _music = music;
            _isSounding = saveData.isSounding;
            _musicVolumePercent = saveData.musicVolumePercent;
            _soundsVolumePercent = saveData.soundsVolumePercent;
        }
        public void SetMusicVolumePercent(sbyte volumePercent)
        {
            if (volumePercent < 0)
                _musicVolumePercent = 0;
            else if (volumePercent > 100)
                _musicVolumePercent = 100;
            else
                _musicVolumePercent = volumePercent;
            MediaPlayer.Volume = GetMusicVolume();
        }
        public void Play()
        {
            MediaPlayer.Play(_music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = GetMusicVolume();
            if (_isSounding == false)
            {
                Pause();
            }
        }
        public void EditIsSounding(bool isSounding)
        {
            _isSounding = isSounding;
            if (isSounding)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        public void SetIsSounding(bool isSounding)
        {
            _isSounding = isSounding;
        }
        public bool GetIsSounding()
        {
            return _isSounding;
        }
        public string GetMusicVolumePercentString()
        {
            return _musicVolumePercent.ToString() + " %";
        }
        public void IncMusicVolume()
        {
            _musicVolumePercent += VOLUME_STEP;
            if (_musicVolumePercent > 100)
            {
                _musicVolumePercent = 100;
            }
            MediaPlayer.Volume = GetMusicVolume();
        }
        public void DecMusicVolume()
        {
            _musicVolumePercent -= VOLUME_STEP;
            if (_musicVolumePercent < 0)
            {
                _musicVolumePercent = 0;
            }
            MediaPlayer.Volume = GetMusicVolume();
        }

        private void Pause()
        {
            MediaPlayer.Pause();
        }
        private void Resume()
        {
            MediaPlayer.Resume();
        }
        public sbyte GetMusicVolumePercent()
        {
            return _musicVolumePercent;
        }
        private float GetMusicVolume()
        {
            return _musicVolumePercent / 100.0f;
        }

        public void PlaySoundEffect(SoundEffect soundEffect)
        {
            if (GetIsSounding())
            {
                soundEffect.Play(GetSoundsVolume(), 0.5f, 0f);
            }
        }
        public sbyte GetSoundsVolumePercent()
        {
            return _soundsVolumePercent;
        }
        private float GetSoundsVolume()
        {
            return _soundsVolumePercent / 100.0f;
        }
        public string GetSoundsVolumePercentString()
        {
            return _soundsVolumePercent.ToString() + " %";
        }
        public void IncSoundsVolume()
        {
            _soundsVolumePercent += VOLUME_STEP;
            if (_soundsVolumePercent > 100)
            {
                _soundsVolumePercent = 100;
            }
        }
        public void DecSoundsVolume()
        {
            _soundsVolumePercent -= VOLUME_STEP;
            if (_soundsVolumePercent < 0)
            {
                _soundsVolumePercent = 0;
            }
        }
    }
}
