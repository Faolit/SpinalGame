using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New EnemyConfig", menuName = "Config/Create EnemyConfig")]
    public class EnemyConfig : ConfigBase
    {
        [SerializeField] public AssetType asset;
        [SerializeField] public WeaponConfigBase weaponConfig;
        [SerializeField] public MoveConfig moveConfig;
        [SerializeField] public int score;
        [SerializeField] public int maxHealth;
        [SerializeField] public float lifeTime;
    }
}
