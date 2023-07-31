using UnityEngine;

namespace SpinalPlay
{
    public interface ITrajectory
    {
        public Vector2 CalculateMove(Vector2 currentPos, Vector2 currentDirection, MoveConfig config);
    }
}
