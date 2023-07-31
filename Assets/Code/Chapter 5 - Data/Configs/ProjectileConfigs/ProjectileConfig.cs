using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New ProjectileConfig", menuName = "Config/Create ProjectileConfig")]
    public class ProjectileConfig : ConfigBase
    {
        [SerializeField] public bool onCollisionDestroyed;
        [SerializeField] public AssetType assetType;
        [SerializeField] public int damage;
        [SerializeField] public MoveConfig moveConfig;
        [SerializeField] public float lifeTime;
    }
}