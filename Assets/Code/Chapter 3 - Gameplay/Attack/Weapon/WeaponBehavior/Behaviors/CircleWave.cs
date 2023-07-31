using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class CircleWave : BarrageBase
    {
        public CircleWave(PoolService poolService, ProjectileConfig config, UnitTag damageTag, EventBus eventBus)
        : base(poolService, config, damageTag, eventBus){}

        public override IEnumerator StartBarrage(Vector2 firePos)
        {
            foreach(Vector2 dir in GetCircleVectors(16))
            {
                yield return PrewarmNewObj();
                SetPosAndDir(firePos, dir);
            }
        }

        public static Vector2[] GetCircleVectors(int numberOfVectors)
        {
            if (numberOfVectors <= 0)
            {
                return null;
            }

            Vector2[] vectors = new Vector2[numberOfVectors];
            float angleStep = 360f / numberOfVectors;
            float currentAngle = 0f;

            for (int i = 0; i < numberOfVectors; i++)
            {
                float x = Mathf.Cos(Mathf.Deg2Rad * currentAngle);
                float y = Mathf.Sin(Mathf.Deg2Rad * currentAngle);

                vectors[i] = new Vector2(x, y).normalized;

                currentAngle += angleStep;
            }

            return vectors;
        }

    }
}
