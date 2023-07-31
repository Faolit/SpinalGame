using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;

namespace SpinalPlay
{
    public class GameObjectFactory : FactoryBase, IService
    {
        private readonly ConfigDirectoryService _configDirectory;
        private readonly PoolService _poolService;
        private readonly EventBus _eventBus;
        private readonly MusicService _music;

        private readonly ShipsData _shipsData;
        private readonly LevelsData _choosedLevel;

        private InputActionAsset _inputActionAsset;

        private const float BOUNDS_OFFSET = 1.5f;

        public GameObjectFactory(AssetProviderService assets, AssetAddresses references, ConfigDirectoryService configDirectory, PoolService poolService, EventBus eventBus, DataTransferService data, MusicService music)
        {
            _assets = assets;
            _references = references;
            _configDirectory = configDirectory;
            _poolService = poolService;
            _eventBus = eventBus;
            _music = music;

            _shipsData = data.Get<ShipsData>();
            _choosedLevel = data.Get<LevelsData>();
        }

        public async Task CreateLevel()
        {
            LevelConfig levelConfig = await _configDirectory.GetConfig<LevelConfig>(_choosedLevel.CurrentLevelId) as LevelConfig;

            SkyObject[] allSkies = CreateSkies(levelConfig.skyConfigs);
            CreateBackground(allSkies);

            CreateSpavners(levelConfig.spavnerConfigs);

            CreateSpavnerMonitor(levelConfig.spavnerConfigs.Length);

            CreateLevelBounds();

            await _music.PlayMusic(levelConfig.music);
        } 

        public async Task<GameObject> CreateShip()
        {
            ShipConfig shipConfig =  await _configDirectory.GetConfig<ShipConfig>(_shipsData.currentShipId) as ShipConfig;
            GameObject ship = await AsyncInstantiate(shipConfig.assetType);

            int weaponID = _shipsData.ShipToWeapID(shipConfig.ID);

            WeaponConfig weaponConfig = _configDirectory.GetConfig<WeaponConfig>(weaponID).Result as WeaponConfig;
            GameObject weapon = await CreateWeapon(weaponConfig, UnitTag.Player);

            AttackComponent attackComponent = ship.GetComponent<AttackComponent>();
            attackComponent.Initialize(await GetInputAction(), weapon);

            MovePlayerComponent moveComponent = ship.GetComponent<MovePlayerComponent>();
            moveComponent.Initialize(shipConfig, await GetInputAction());

            HealthComponent healthComponent = ship.GetComponent<HealthComponent>();
            healthComponent.Inititalize(shipConfig.maxHp, _eventBus, UnitTag.Player);

            return ship;
        }

        public async Task<GameObject> CreateEmptyShip(ShipConfig shipConfig)
        {
            GameObject ship = await AsyncInstantiate(shipConfig.assetType);
            ship.SetActive(false);
            return ship;
        }

        public async Task<GameObject> CreateEmptyWeapon(WeaponConfig weaponConfig)
        {
            GameObject weapon = await AsyncInstantiate(weaponConfig.assetType);
            weapon.SetActive(false);
            return weapon;
        }

        public async Task<GameObject> CreateCloudSpavner(Vector2 startPos, float maxYOffset, float spavnDelay)
        {

            GameObject cloudSpavner = await AsyncInstantiate(AssetType.CloudSpavner);

            CloudSpavner cloudComp = cloudSpavner.GetComponent<CloudSpavner>();
            cloudComp.Initialize(_poolService, startPos, maxYOffset, spavnDelay);

            return cloudSpavner;
        }

        private GameObject[] CreateSpavners(SpavnerConfig[] configs)
        {
            GameObject[] spavners = new GameObject[configs.Length];

            for (int i = 0; i < spavners.Length; i++)
            {
                spavners[i] = CreateSpavner(configs[i]);
            }

            return spavners;
        }

        private GameObject CreateBackground(SkyObject[] skies)
        {
            GameObject background = new GameObject("Background");
            SkyMover backComp = background.AddComponent<SkyMover>();
            backComp.Initialize(skies);

            return background;
        }

        private SkyObject[] CreateSkies(SkyConfig[] configs)
        {
            SkyObject[] allSkies = new SkyObject[configs.Length];

            for (int i = 0; i < configs.Length; i++)
            {
                allSkies[i] = CreateSky(configs[i]);
            }

            return allSkies;
        }

        private SkyObject CreateSky(SkyConfig config)
        {
            GameObject skyGO = new GameObject();

            SpriteRenderer sprite = skyGO.AddComponent<SpriteRenderer>();
            sprite.drawMode = SpriteDrawMode.Tiled;
            sprite.sprite = config.sprite;
            sprite.sortingOrder = config.order;

            skyGO.transform.localScale = new Vector2(config.scale, config.scale);

            SkyObject skyObj = new SkyObject(skyGO, config.speed);
            return skyObj;
        }

        private async Task<GameObject> CreateWeapon(WeaponConfig config, UnitTag unitTag)
        {
            GameObject weapon = await AsyncInstantiate(config.assetType);
            WeaponComponent weaponComponent = weapon.GetComponent<WeaponComponent>();
            weaponComponent.Initialize(_poolService, config, unitTag, _eventBus);
            return weapon;
        }

        private GameObject CreateSpavner(SpavnerConfig spavnerConfig)
        {
            GameObject spavner = new GameObject($"Spawner");
            EnemySpawnerComponent spavnComponent = spavner.AddComponent<EnemySpawnerComponent>();
            spavnComponent.Initialize(spavnerConfig, _poolService, _eventBus);
            spavnComponent.InvokeSpawn();

            return spavner;
        }

        private void CreateLevelBounds()
        {
            foreach(BoundData data in CameraCalc.GetCameraBoundsData(BOUNDS_OFFSET))
            {
                CreateBound(data.pos, data.sizeX, data.sizeY);
            }
        }

        private void CreateBound(Vector2 pos, float sizeX = 1, float sizeY = 1)
        {
            GameObject gameObject = new GameObject("Bound");
            gameObject.transform.position = pos;
            gameObject.layer = 7;

            Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            rb.useFullKinematicContacts = true;

            BoxCollider2D collider =  gameObject.AddComponent<BoxCollider2D>();
            collider.size = new Vector3(sizeX, sizeY);
        }

        private GameObject CreateSpavnerMonitor(int spawnerCount)
        {
            GameObject monitor = new GameObject("SpawnerMonitor");

            SpawnerMonitor monitorComp = monitor.AddComponent<SpawnerMonitor>();
            monitorComp.Initialize(spawnerCount, _eventBus, _poolService);

            return monitor;
        }

        private async Task<InputActionAsset> GetInputAction()
        {
            if (_inputActionAsset == null)
            {
                AssetReference reference = _references.GetAssetReference(AssetType.DefaultAction);
                _inputActionAsset = await _assets.GetAsset<InputActionAsset>(reference);
            }
            return _inputActionAsset;
        }
    }
}