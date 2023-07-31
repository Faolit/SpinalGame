using UnityEngine;

namespace SpinalPlay
{
    [CreateAssetMenu(fileName = "New MoveConfig", menuName = "Config/Create MoveConfig")]
    public class MoveConfig : ConfigBase
    {
        [SerializeField] public float speed;
        [SerializeField] public TrajectoryType trajectoryType;
    }
}
