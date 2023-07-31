using System;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class UIFactory : FactoryBase, IService
    {
        private readonly EventBus _eventBus;
        private readonly ConfigDirectoryService _configDirectory;
        private readonly GameObjectFactory _factory;

        private readonly LevelsData _levelsData;
        private readonly ShipsData _shipsData;
        private readonly PlayerMoneyData _moneyData;
        private readonly WeaponData _weaponData;
        private readonly SettingData _settingData;

        public UIFactory(AssetProviderService assets, AssetAddresses references, EventBus eventBus, ConfigDirectoryService configDirectory, GameObjectFactory objectFactory, DataTransferService data)
        {
            _assets = assets;
            _references = references;
            _eventBus = eventBus;
            _configDirectory = configDirectory;
            _factory = objectFactory;

            _levelsData = data.Get<LevelsData>();
            _shipsData = data.Get<ShipsData>();
            _moneyData = data.Get<PlayerMoneyData>();
            _weaponData = data.Get<WeaponData>();
            _settingData = data.Get<SettingData>();
        }

        public async Task<GameObject> CreateLoadingCurtain()
        {
            GameObject curtain = await AsyncInstantiate(AssetType.LoadingCanvas);
            return curtain;
        }

        public async Task<GameObject> CreateRootCanvas(Camera camera, int orderInLayer)
        {
            GameObject canvas = await AsyncInstantiate(AssetType.RootCanvas);

            Canvas comp = canvas.GetComponent<Canvas>();
            comp.worldCamera = camera;
            comp.sortingOrder = orderInLayer;

            return canvas;
        }

        public async Task<GameObject> CreateOverCanvas()
        {
            GameObject canvas = await AsyncInstantiate(AssetType.OverCanvas);
            return canvas;
        }

        public async Task<GameObject> CreateMainMenuContainer(Transform parent)
        {
            GameObject container = await AsyncInstantiate(AssetType.MainMenuContainer, parent);
            container.GetComponent<MainMenuContainer>().Initialize(_eventBus);
            return container;
        }

        public async Task<GameObject> CreateSettingWindow(Transform parent)
        {
            GameObject window = await AsyncInstantiate(AssetType.SettingWindow, parent);
            window.GetComponent<SettingWindow>().Initialize(_eventBus, _settingData);
            return window;
        }

        public async Task<GameObject> CreateWarningWindow(Transform parent)
        {
            GameObject window = await AsyncInstantiate(AssetType.WarningWindow, parent);
            window.GetComponent<WarningWindow>().Initialize(_eventBus);
            return window;
        }

        public async Task<GameObject> CreateNotificationWindow(Transform parent)
        {
            GameObject window = await AsyncInstantiate(AssetType.NotificationWindow, parent);
            window.GetComponent<NotificationWindow>().Initialize(_eventBus);
            return window;
        }

        public async Task<GameObject> CreatePreparationSceneContainer(Transform parent)
        {
            GameObject container = await AsyncInstantiate(AssetType.PreparationContainer, parent);
            PreparationSceneContainer contComp = container.GetComponent<PreparationSceneContainer>();
            contComp.Initialize(_eventBus, _levelsData);
            return container;
        }

        public async Task<GameObject> CreateShipShopWindow(GameObject canvas)
        {
            GameObject window = await AsyncInstantiate(AssetType.ShipShopWindow, canvas.transform);
            ChooseShipWindow winComponent = window.GetComponent<ChooseShipWindow>();
            await winComponent.Initialize(_configDirectory, _factory, _shipsData, _moneyData, _eventBus);
            return window;
        }

        public async Task<GameObject> CreateWeaponShopWindow(GameObject canvas)
        {
            GameObject window = await AsyncInstantiate(AssetType.WeaponShopWindow, canvas.transform);
            ChooseWeaponWindow winComp = window.GetComponent<ChooseWeaponWindow>();
            await winComp.Initialize(_configDirectory, _factory, _weaponData, _shipsData, _moneyData, _eventBus);
            return window;
        }

        public async Task<GameObject> CreateCamera()
        {
            GameObject camera = await AsyncInstantiate(AssetType.Camera);
            ShakeCameraEffect shaker = camera.GetComponent<ShakeCameraEffect>();
            shaker.Initialize(_eventBus);
            return camera;
        }

        public async Task<GameObject> CreateGameContainer(Transform parent)
        {
            GameObject container = await AsyncInstantiate(AssetType.GameContainer, parent);

            int currentLevel = _levelsData.CurrentLevelId;
            LevelConfig levelConfig = await _configDirectory.GetConfig<LevelConfig>(currentLevel);
            int currentShip = _shipsData.currentShipId;
            ShipConfig shipConfig = await _configDirectory.GetConfig<ShipConfig>(currentShip);

            container.GetComponent<GameContainer>().Initialize(_eventBus, _levelsData, shipConfig.maxHp, levelConfig);

            return container;
        }

        public async Task<GameObject> CreatePauseWindow(Transform parent)
        {
            GameObject window = await AsyncInstantiate(AssetType.PauseWindow, parent.transform);

            PauseWindow winComp = window.GetComponent<PauseWindow>();
            winComp.Initialize(_eventBus);

            return window;
        }

        public async Task<GameObject> CreateGameResultWindow(Transform parent)
        {
            GameObject window = await AsyncInstantiate(AssetType.GameResultWindow, parent.transform);

            GameResultPanel winComp = window.GetComponent<GameResultPanel>();
            int currentLevel = _levelsData.CurrentLevelId;
            LevelConfig levelConfig = await _configDirectory.GetConfig<LevelConfig>(currentLevel);
            winComp.Initialize(_eventBus, levelConfig, _levelsData, _moneyData);

            return window;
        }
    }
}