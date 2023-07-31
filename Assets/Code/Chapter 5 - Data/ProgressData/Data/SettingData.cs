using System;

namespace SpinalPlay
{
    public class SettingData : DataBase
    {
        public delegate void volumeChamgeDelegate(float volume);
        public event volumeChamgeDelegate musicVolChange;
        public event volumeChamgeDelegate soundVolChange;

        public float MusicVolume 
        {
            get {return _musicVolume;}
            set 
            {
                _musicVolume = UnityEngine.Mathf.Clamp01(value);
                musicVolChange?.Invoke(_musicVolume);
            } 
        }

        public float SoundVolume
        {
            get { return _soundVolume; }
            set 
            {
                _soundVolume = UnityEngine.Mathf.Clamp01(value); 
                soundVolChange?.Invoke(_soundVolume);
            }
        }

        private float _musicVolume;
        private float _soundVolume;

        public SettingData() 
        {
            Reset();
        }

        public override void Reset()
        {
            _musicVolume = 1f;
            _soundVolume = 1f;
        }
    }
}
