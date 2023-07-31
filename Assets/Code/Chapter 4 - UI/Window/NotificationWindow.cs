using TMPro;
using UnityEngine;

namespace SpinalPlay
{
    public class NotificationWindow : WindowBase
    {
        [SerializeField] private TMP_Text notificMessage;

        public new void Initialize(EventBus eventBus)
        {
            base.Initialize(eventBus);
            Subscribe();
        }

        private void OpenWindow(OpenNotificationWindow signal)
        {
            notificMessage.text = signal.message;
            gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<OpenNotificationWindow>(OpenWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OpenNotificationWindow>(OpenWindow);
        }
    }
}
