using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class PauseWindow : WindowBase
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _backToPrepareButton;

        public new void Initialize(EventBus eventBus)
        {
            base.Initialize(eventBus);
            InitButtons();
            Subscribe();
        }

        private void InvokeRestart()
        {
            _eventBus.Invoke<EnterRestarting>(new EnterRestarting());
            Time.timeScale = 1;
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        protected override void CloseWindow()
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void BackToPrepare()
        {
            _eventBus.Invoke<EnterToPrepare>(new EnterToPrepare());
            Time.timeScale = 1;
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void InitButtons()
        {
            _restartButton.onClick.AddListener(InvokeRestart);
            _backToPrepareButton.onClick.AddListener(BackToPrepare);
        }

        private void ShowWindow(GamePaused signal)
        {
            gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<GamePaused>(ShowWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<GamePaused>(ShowWindow);
        }
    }
}