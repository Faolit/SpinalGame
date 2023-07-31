using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public abstract class BarrageBase
    {
        private readonly PoolService _poolService;
        private readonly ProjectileConfig _config;
        private readonly EventBus _eventBus;
        private readonly UnitTag _damageTag;

        private GameObject _projObj;

        public BarrageBase(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus) 
        {
            _poolService = poolService;
            _config = config;
            _damageTag = damageTag;
            _eventBus = eventBus;
        }

        public abstract IEnumerator StartBarrage(Vector2 firePos);

        protected IEnumerator PrewarmNewObj()
        {
            Task<GameObject> task = _poolService.GetBullet(_config);
            yield return new WaitUntil(() => task.IsCompleted);

            _projObj = task.Result;

            DamageComponent damageComponent = _projObj.GetComponent<DamageComponent>();
            damageComponent.Initialize(_config.damage, _damageTag, _config.onCollisionDestroyed, _eventBus);
        }

        protected void SetPosAndDir(Vector2 pos, Vector2 dir)
        {
            AutonomMoveComponent projectileComp = _projObj.GetComponent<AutonomMoveComponent>();
            projectileComp.SetNewWay(pos, dir);

            _projObj.SetActive(true);
        }
    }
}
