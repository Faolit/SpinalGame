using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class WarningWindow : WindowBase
    {
        [SerializeField] private Button OkBtn;
        [SerializeField] private TMP_Text warningMessage;

        private Action _callback;

        public new void Initialize(EventBus eventBus)
        {
            base.Initialize(eventBus);

            Subscribe();
            ButtonInit();
        }

        private void ButtonInit()
        {
            OkBtn.onClick.AddListener(OnOkPressed);
        }

        private void OpenWindow(OpenWarningWindow signal)
        {
            warningMessage.text = signal.message;
            _callback = signal.callback;
            gameObject.SetActive(true);
        }

        private void OnOkPressed()
        {
            _callback.Invoke();
            gameObject.SetActive(false);
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<OpenWarningWindow>(OpenWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OpenWarningWindow>(OpenWindow);
        }
    }
}
