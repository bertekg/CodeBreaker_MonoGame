using Microsoft.Xna.Framework.Media;

namespace CodeBreaker_MonoGame
{
    internal class BackgroundSong
    {
        private Song _music;
        private bool _isSounding;
        private sbyte _volumePercent;

        const sbyte VOLUME_STEP = 10;
        public BackgroundSong(Song music, SaveData saveData)
        {
            _music = music;
            _isSounding = saveData.isSounding;
            _volumePercent = saveData.musicVolumePercent;
        }
        public void SetVolumePercent(sbyte volumePercent)
        {
            if (volumePercent < 0)
                _volumePercent = 0;
            else if (volumePercent > 100)
                _volumePercent = 100;
            else
                _volumePercent = volumePercent;
            MediaPlayer.Volume = GetVolume();
        }
        public void Play()
        {
            MediaPlayer.Play(_music);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = GetVolume();
            if (_isSounding == false)
            {
                Pause();
            }
        }
        public void NewIsSounding(bool isSounding)
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
        public string GetVolumePercentString()
        {
            return _volumePercent.ToString() + " %";
        }
        public void IncVolume()
        {
            _volumePercent += VOLUME_STEP;
            if (_volumePercent > 100)
            {
                _volumePercent = 100;
            }
            MediaPlayer.Volume = GetVolume();
        }
        public void DecVolume()
        {
            _volumePercent -= VOLUME_STEP;
            if (_volumePercent < 0)
            {
                _volumePercent = 0;
            }
            MediaPlayer.Volume = GetVolume();
        }

        private void Pause()
        {
            MediaPlayer.Pause();
        }
        private void Resume()
        {
            MediaPlayer.Resume();
        }
        public sbyte GetVolumePercent()
        {
            return _volumePercent;
        }
        private float GetVolume()
        {
            return (float)_volumePercent / 100.0f;
        }
    }
}
