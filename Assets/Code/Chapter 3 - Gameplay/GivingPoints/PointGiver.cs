using UnityEngine;

namespace SpinalPlay
{
    public class PointGiver : MonoBehaviour
    {
        public int Points { get { return _points; } }
        private int _points;
        public void Initialize(int points)
        {
            _points = points;
        }
    }
}