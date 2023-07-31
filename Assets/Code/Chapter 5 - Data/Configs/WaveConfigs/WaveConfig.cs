using System.Collections.Generic;
using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New WaveConfig", menuName = "Config/Create WaveConfig")]
    public class WaveConfig : ConfigBase
    {
        [SerializeField] public List<EnemyConfig> enemyConfigs;
        [SerializeField] public float enemyDelay;
    }
}
