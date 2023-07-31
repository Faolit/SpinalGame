using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New spavnerConfig", menuName = "Config/Create SpavnerConfig")]
    public class SpavnerConfig : ConfigBase
    {
        [SerializeField] public Vector2 spavnerPosition;
        [SerializeField] public WaveConfig[] waveConfigs;
        [SerializeField] public float waveDelay;
        [SerializeField] public float randomPos;
    }
}
