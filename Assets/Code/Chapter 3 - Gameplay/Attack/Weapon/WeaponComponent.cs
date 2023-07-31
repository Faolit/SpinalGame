using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class WeaponComponent : MonoBehaviour
    {
        [SerializeField] private ProjectileConfig _bulletConfig;
        [SerializeField] private Transform _firePoint;

        private WeaponConfigBase _config;
        private EventBus _eventBus; 
        private BarrageBase _barrage;
        
        public void Initialize(PoolService pool, WeaponConfigBase config, UnitTag unitTag, EventBus eventBus)
        {
            _config = config;
            _eventBus = eventBus;

            BarrageFactory factory = new BarrageFactory(pool, _bulletConfig, unitTag, eventBus);
            _barrage = factory.CreateBarrage(_config.barrageType);
        }

        public IEnumerator Attack()
        {
            _eventBus.Invoke<InvokeSound>(new InvokeSound(AssetType.LaserFireSound));
            StartCoroutine(_barrage.StartBarrage(_firePoint.position));
            yield return new WaitForSeconds(_config.fireDelay);
        }
    }
}