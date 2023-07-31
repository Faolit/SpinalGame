using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New WeaponConfig", menuName = "Config/Create WeaponConfig")]
    public class WeaponConfig : WeaponConfigBase
    {
        [SerializeField] public string weaponName;
        [SerializeField] public string description;
        [SerializeField] public int cost;
    }
}