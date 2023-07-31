using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class ChooseWeaponWindow : ShopWindowsBase
    {
        private WeaponData _weaponData;
        private ShipsData _shipsData;
        private PlayerMoneyData _moneyData;

        private ConfigDirectoryService _configDirectoryService;
        private GameObjectFactory _objectFactory;

        private GameObject[] _views;
        private WeaponConfig[] _weaponConfigs;

        public async Task Initialize(ConfigDirectoryService configDirectory, GameObjectFactory objectFactory, WeaponData weaponData, ShipsData shipsData, PlayerMoneyData playerMoneyData, EventBus eventBus)
        {
            _configDirectoryService = configDirectory;
            _objectFactory = objectFactory;
            _eventBus = eventBus;

            _shipsData = shipsData;
            _moneyData = playerMoneyData;
            _weaponData = weaponData;

            base.Initialize(eventBus);

            await GetAllConfigs();
            SortWeaponConfig();
            await GetAllView();

            UpdateShopCard();
            UpdateMoneyView();
            FitButtons(CheckAvailables());

            Subscribe();
        }

        protected override async Task GetAllView()
        {
            List<GameObject> views = new List<GameObject>();

            foreach (WeaponConfig config in _weaponConfigs)
            {
                views.Add(await _objectFactory.CreateEmptyWeapon(config));
            }
            _views = views.ToArray();
        }

        private async Task GetAllConfigs()
        {
            _weaponConfigs = await _configDirectoryService.GetConfigs<WeaponConfig>();

            _maxIndex = _weaponConfigs.Length - 1;
        }

        protected override bool CheckAvailables()
        {
            int id = _weaponConfigs[_itemIndex].ID;
            if (_weaponData.activeWeapon.Contains(id))
            {
                return true;
            }
            return false;
        }

        protected override void SetItem()
        {
            _shipsData.shipToWeaponId[_shipsData.currentShipId] = _weaponConfigs[_itemIndex].ID;
            CloseWindow();
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.BtnClick));
        }

        protected override void TryBuyItem()
        {
            if (_moneyData.money >= _weaponConfigs[_itemIndex].cost)
            {
                _weaponData.activeWeapon.Add(_weaponConfigs[_itemIndex].ID);
                _moneyData.money -= _weaponConfigs[_itemIndex].cost;
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
            _shopCard.UpdateCard(_weaponConfigs[_itemIndex], _views[_itemIndex]);
        }

        protected override void UpdateMoneyView()
        {
            _moneyView.text = $"You hawe {_moneyData.money} research score";}

        private void ShowWindow(OpenWeaponShopWindow signal)
        {
            UpdateMoneyView();
            gameObject.SetActive(true);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<OpenWeaponShopWindow>(ShowWindow);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<OpenWeaponShopWindow>(ShowWindow);
        }

        private void SortWeaponConfig()
        {
            _weaponConfigs = _weaponConfigs.OrderBy(weapon => weapon.cost).ToArray();
        }
    }
}