using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class TripleFire : BarrageBase
    {
        private const float NEXT_OFFSET = 0.5f;
        private const float FIRE_OFFSET = -0.5f;

        public TripleFire(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus)
        : base(poolService, config, damageTag, eventBus) { }

        public override IEnumerator StartBarrage(Vector2 firePos)
        {
            for(int i = 0; i < 3; i++)
            {
                yield return PrewarmNewObj();

                Vector2 nextPos = firePos;
                nextPos.x += FIRE_OFFSET + NEXT_OFFSET * i;

                SetPosAndDir(nextPos, Vector2.up);
            }
        }
    }
}
