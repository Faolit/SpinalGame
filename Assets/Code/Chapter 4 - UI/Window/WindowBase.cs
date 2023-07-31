using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class WindowBase : MonoBehaviour
    {
        [SerializeField] private Button _closeWindowButton;
        protected EventBus _eventBus;

        protected void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;
            _closeWindowButton.onClick.AddListener(CloseWindow);
        }

        protected virtual void CloseWindow() 
        {
            gameObject.SetActive(false);
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }
    }
}