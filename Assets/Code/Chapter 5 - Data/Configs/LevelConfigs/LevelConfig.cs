using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New LevelConfig", menuName = "Config/Create LevelConfig")]
    public class LevelConfig : ConfigBase
    {
        [SerializeField] public SkyConfig[] skyConfigs;
        [SerializeField] public SpavnerConfig[] spavnerConfigs;
        [SerializeField] public int minWinScore;
        [SerializeField] public AssetType music;
    }
}
