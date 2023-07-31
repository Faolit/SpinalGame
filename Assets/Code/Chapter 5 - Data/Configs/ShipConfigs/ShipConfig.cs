using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New ShipConfig", menuName = "Config/Create ShipConfig")]
    public class ShipConfig : ConfigBase
    {
        [SerializeField] public AssetType assetType;
        [SerializeField] public string shipName;
        [SerializeField] public string shipDescription;
        [SerializeField] public int shipCost;
        [SerializeField] public int maxHp;
        [SerializeField] public MoveConfig moveConfig;
    }
}
