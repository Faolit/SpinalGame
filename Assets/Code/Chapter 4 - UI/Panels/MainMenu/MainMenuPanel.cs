using System;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class MainMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        [SerializeField] private Button _settingButton;

        [SerializeField] private Button _newGameButton;

        [SerializeField] private Button _exitButton;

        private EventBus _eventBus;

        public void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;

            _startButton.onClick.AddListener(StartGame);
            _settingButton.onClick.AddListener(OpenSettingPanel);
            _newGameButton.onClick.AddListener(StartNewGame);
            _exitButton.onClick.AddListener(ExitGame);
        }

        private void StartGame()
        {
            _eventBus.Invoke<EnterToPrepare>(new EnterToPrepare());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void OpenSettingPanel()
        {
            _eventBus.Invoke<OpenSettingWindow>(new OpenSettingWindow());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void ExitGame()
        {
            Action callback = () => Application.Quit();
            _eventBus.Invoke<OpenWarningWindow>(new OpenWarningWindow(callback, "Are you sure you want to leave the game?"));
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void StartNewGame()
        {
            Action callback = () => _eventBus.Invoke<NewGame>(new NewGame()); 
            _eventBus.Invoke<OpenWarningWindow>(new OpenWarningWindow(callback, "Are you sure you want to start new game?"));
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }
    }
}