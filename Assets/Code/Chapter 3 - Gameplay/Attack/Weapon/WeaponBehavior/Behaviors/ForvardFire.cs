using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class ForvardFire : BarrageBase
    {
        public ForvardFire(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus)
        : base(poolService, config, damageTag, eventBus){}

        public override IEnumerator StartBarrage(Vector2 firePos)
        {
            yield return PrewarmNewObj();
            SetPosAndDir(firePos, Vector2.up);
        }
    }
}