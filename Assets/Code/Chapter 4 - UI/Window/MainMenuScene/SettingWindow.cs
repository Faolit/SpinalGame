using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class SettingWindow : WindowBase
    {
        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _soundSlider;

        private SettingData _settingData;

        public void Initialize(EventBus eventBus, SettingData settingData)
        {
            _eventBus = eventBus;
            _settingData = settingData;
            base.Initialize(eventBus);
            InitSliders();
            Subscribe();
        }

        private void InitSliders()
        {
            _musicSlider.onValueChanged.AddListener(UpdateMusicValue);
            _soundSlider.onValueChanged.AddListener(UpdateSoundValue);
        }

        private void UpdateMusicValue(float value)
        {
            if (_settingData != null)
            {
                _settingData.MusicVolume = value;
            }
        }

        private void UpdateSoundValue(float value)
        {
            if (_settingData != null)
            {
                _settingData.SoundVolume = value;
            }
        }

        private void OpenWindow(OpenSettingWindow signal)
        {
            gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<OpenSettingWindow>(OpenWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OpenSettingWindow>(OpenWindow);
        }
    }
}
