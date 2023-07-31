using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New WeaponConfigBase", menuName = "Config/Create WeaponConfigBase")]
    public class WeaponConfigBase : ConfigBase
    {
        [SerializeField] public AssetType assetType;
        [SerializeField] public BarrageType barrageType;
        [SerializeField] public float fireDelay;
    }
}
