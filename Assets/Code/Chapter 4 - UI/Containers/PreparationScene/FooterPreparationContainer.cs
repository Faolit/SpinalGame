using UnityEngine;
using UnityEngine.UI;

namespace SpinalPlay
{
    public class FooterPreparationContainer : MonoBehaviour
    {
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _openWeaponShopButton;
        [SerializeField] private Button _openChooseShipButton;
        [SerializeField] private Button _startLevelButton;

        private EventBus _eventBus;
        public void Initialize(EventBus eventBus)
        {
            _exitButton.onClick.AddListener(OnExitPressed);
            _openWeaponShopButton.onClick.AddListener(OnWeaponShopPressed);
            _openChooseShipButton.onClick.AddListener(OnChooseShipPressed);
            _startLevelButton.onClick.AddListener(OnStartLevelPressed);

            _eventBus = eventBus;
        }

        private void  OnExitPressed()
        {
            _eventBus.Invoke<EnterToMainMenu>(new EnterToMainMenu());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void OnWeaponShopPressed()
        {
            _eventBus.Invoke<OpenWeaponShopWindow>(new OpenWeaponShopWindow());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void OnChooseShipPressed() 
        {
            _eventBus.Invoke<OpenShipShopWindow>(new OpenShipShopWindow());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        private void OnStartLevelPressed() 
        {
            _eventBus.Invoke<EnterToGameLoop>(new EnterToGameLoop());
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }
    }
}
