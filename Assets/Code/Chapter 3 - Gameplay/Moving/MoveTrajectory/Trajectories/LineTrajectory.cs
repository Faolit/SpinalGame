using UnityEngine;

namespace SpinalPlay
{
    public class LineTrajectory : ITrajectory
    {
        public Vector2 CalculateMove(Vector2 currentPos, Vector2 currentDirection, MoveConfig config)
        {
            Vector2 newPos = currentPos + currentDirection * config.speed * Time.fixedDeltaTime;
            return newPos;
        }
    }
}
