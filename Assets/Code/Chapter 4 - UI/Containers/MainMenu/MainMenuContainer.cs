using UnityEngine;

namespace SpinalPlay
{
    public class MainMenuContainer : MonoBehaviour
    {
        [SerializeField] private MainMenuPanel _panel;
        public void Initialize(EventBus eventBus)
        {
            _panel.Initialize(eventBus);
        }
    }
}