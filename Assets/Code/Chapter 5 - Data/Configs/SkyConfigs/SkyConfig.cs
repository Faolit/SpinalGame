using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New SkyConfig", menuName = "Config/Create SkyConfig")]
    public class SkyConfig : ConfigBase
    {
        [SerializeField] public Sprite sprite;
        [SerializeField] public float scale;
        [SerializeField] public float speed;
        [SerializeField] public int order;
    }
}