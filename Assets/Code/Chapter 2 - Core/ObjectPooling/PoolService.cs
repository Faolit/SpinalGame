using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class PoolService : FactoryBase, IService
    {
        private EventBus _eventBus;
        private Dictionary<ProjectileConfig, GameObjectsPool<ProjectileConfig>> _bullets;
        private Dictionary<EnemyConfig, GameObjectsPool<EnemyConfig>> _enemies;
        private Dictionary<GameObject, SimpleGameObjectPool> _clouds;

        public PoolService(EventBus eventBus, AssetAddresses references, AssetProviderService providerService)
        {
            _eventBus = eventBus;
            _references = references;
            _assets = providerService;

            _enemies = new Dictionary<EnemyConfig, GameObjectsPool<EnemyConfig>>();
            _bullets = new Dictionary<ProjectileConfig, GameObjectsPool<ProjectileConfig>>();
            _clouds = new Dictionary<GameObject, SimpleGameObjectPool>();

            Subscribe();
        }

        public bool HasAnyEnemies()
        {
            foreach(var enemyType in _enemies.Values)
            {
                if (enemyType.HasAnyActive())
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<GameObject> GetEnemy(EnemyConfig config)
        {
            if (_enemies.ContainsKey(config))
            {
                return await _enemies[config].Get();
            }
            _enemies[config] = new GameObjectsPool<EnemyConfig>(10, CreateEnemy, config);
            return await _enemies[config].Get();
        }

        public async Task<GameObject> GetBullet(ProjectileConfig config)
        {
            if (_bullets.ContainsKey(config))
            {
                return await _bullets[config].Get();
            }
            _bullets[config] = new GameObjectsPool<ProjectileConfig>(10, CreateBullet, config);
            return await _bullets[config].Get();
        }

        public GameObject GetCloud(GameObject cloudPref, MoveConfig moveConfig)
        {
            if(_clouds.ContainsKey(cloudPref))
            {
                return _clouds[cloudPref].Get();
            }
            _clouds[cloudPref] = new SimpleGameObjectPool(4, () => CreateCloud(cloudPref, moveConfig));
            return _clouds[cloudPref].Get();
        } 

        private async Task<GameObject> CreateBullet(ProjectileConfig config)
        {
            GameObject projectileObj = await AsyncInstantiate(config.assetType);

            AutonomMoveComponent projectileComp = projectileObj.GetComponent<AutonomMoveComponent>();
            projectileComp.Initialize(config.moveConfig);

            ProjWithLifeTime lifeTime = projectileObj.GetComponent<ProjWithLifeTime>();
            lifeTime.Initialize(config.lifeTime);

            projectileObj.SetActive(false);

            return projectileObj;
        }

        private async Task<GameObject> CreateEnemy(EnemyConfig config)
        {
            GameObject enemyObj = await AsyncInstantiate(config.asset);
            enemyObj.SetActive(false);

            AutonomMoveComponent moveComp = enemyObj.GetComponent<AutonomMoveComponent>();
            moveComp.Initialize(config.moveConfig);

            HealthComponent health = enemyObj.GetComponent<HealthComponent>();
            health.Inititalize(config.maxHealth, _eventBus, UnitTag.Enemy);

            AutonomAttackComponent attComp = enemyObj.GetComponent<AutonomAttackComponent>();
            attComp.Initialize(await CreateWeapon(config.weaponConfig, UnitTag.Enemy));

            PointGiver pointGiver = enemyObj.GetComponent<PointGiver>();
            pointGiver.Initialize(config.score);

            ProjWithLifeTime lifeTime = enemyObj.GetComponent<ProjWithLifeTime>();
            lifeTime.Initialize(config.lifeTime);

            return enemyObj;
        }

        private GameObject CreateCloud(GameObject cloudPref, MoveConfig moveConfig)
        {
            GameObject cloud = GameObject.Instantiate(cloudPref);

            AutonomMoveComponent moveComp = cloud.GetComponent<AutonomMoveComponent>();
            moveComp.Initialize(moveConfig);

            ProjWithLifeTime lifeTime = cloud.GetComponent<ProjWithLifeTime>();
            lifeTime.Initialize(30);

            return cloud;
        }

        private async Task<GameObject> CreateWeapon(WeaponConfigBase config, UnitTag unitTag)
        {
            GameObject weapon = await AsyncInstantiate(config.assetType);

            WeaponComponent weaponComponent = weapon.GetComponent<WeaponComponent>();
            weaponComponent.Initialize(this, config, unitTag, _eventBus);

            return weapon;
        }

        private void Release(IGOSignal signal)
        {
            signal.Object.SetActive(false);
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<EnemyDead>(Release);
            _eventBus.Subscribe<ProjectileDestroy>(Release);
        }
    }
}