using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class BackvardFire : BarrageBase
    { 
        public BackvardFire(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus)
        : base(poolService, config, damageTag, eventBus) { }

        public override IEnumerator StartBarrage(Vector2 firePos)
        {
            yield return PrewarmNewObj();
            SetPosAndDir(firePos, Vector2.down);
        }
    }
}
