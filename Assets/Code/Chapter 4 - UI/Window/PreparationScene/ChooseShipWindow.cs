using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class ChooseShipWindow : ShopWindowsBase
    {
        private ShipsData _shipsData;
        private PlayerMoneyData _moneyData;

        private ConfigDirectoryService _configDirectoryService;
        private GameObjectFactory _objectFactory;

        private GameObject[] _views;
        private ShipConfig[] _shipConfigs;

        public async Task Initialize(ConfigDirectoryService configDirectory, GameObjectFactory objectFactory, ShipsData shipsData, PlayerMoneyData playerMoneyData, EventBus eventBus)
        {
            _configDirectoryService = configDirectory;
            _objectFactory = objectFactory;

            _shipsData = shipsData;
            _moneyData = playerMoneyData;   

            base.Initialize(eventBus);

            await GetAllConfigs();
            SortShipConfig();
            await GetAllView();

            UpdateShopCard();
            UpdateMoneyView();
            FitButtons(CheckAvailables());

            Subscribe();
        }
        
        protected override async Task GetAllView()
        {
            List<GameObject> views = new List<GameObject>();
            foreach (ShipConfig config in _shipConfigs)
            {
                views.Add(await _objectFactory.CreateEmptyShip(config));
            }
            _views = views.ToArray();
        }

        private async Task GetAllConfigs()
        {
            _shipConfigs = await _configDirectoryService.GetConfigs<ShipConfig>();
            _maxIndex = _shipConfigs.Length - 1;
        }

        protected override bool CheckAvailables()
        {
            int id = _shipConfigs[_itemIndex].ID;
            if (_shipsData.availableShips.Contains(id))
            {
                return true;
            }
            return false;
        }

        protected override void SetItem()
        {
            _shipsData.currentShipId = _shipConfigs[_itemIndex].ID;
            CloseWindow();
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        protected override void TryBuyItem()
        {
            if (_moneyData.money >= _shipConfigs[_itemIndex].shipCost) 
            {
                _shipsData.availableShips.Add(_shipConfigs[_itemIndex].ID);
                _moneyData.money -= _shipConfigs[_itemIndex].shipCost;
                UpdateMoneyView();
                FitButtons(true);
            }
            else
            {
                _eventBus.Invoke<OpenNotificationWindow>(new OpenNotificationWindow("You dont have enough money"));
            }
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        protected override void UpdateShopCard()
        {
            _shopCard.UpdateCard(_shipConfigs[_itemIndex], _views[_itemIndex]);
        }

        protected override void UpdateMoneyView()
        {
            _moneyView.text = $"You hawe {_moneyData.money} research score";
        }

        private void ShowWindow(OpenShipShopWindow signal)
        {
            UpdateMoneyView();
            gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<OpenShipShopWindow>(ShowWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OpenShipShopWindow>(ShowWindow);
        }

        private void SortShipConfig()
        {
            _shipConfigs = _shipConfigs.OrderBy(ship => ship.shipCost).ToArray();
        }
    }
}