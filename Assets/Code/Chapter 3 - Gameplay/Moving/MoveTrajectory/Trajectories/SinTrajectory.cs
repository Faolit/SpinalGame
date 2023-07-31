using UnityEngine;

namespace SpinalPlay
{
    public class SinTrajectory : IReUsedTrajectory
    {
        private float _initX;
        private bool _isInit;

        private void InitX(float initX)
        {
            if (!_isInit)
            {
                _initX = initX;
                _isInit = true;
            }
        }

        public Vector2 CalculateMove(Vector2 currentPos, Vector2 currentDirection, MoveConfig config)
        {
            InitX(currentPos.x);

            float distanceToMove = config.speed * Time.fixedDeltaTime;
            Vector2 nextPosition = currentPos + currentDirection * distanceToMove;
            nextPosition.x = Mathf.Sin(nextPosition.y) + _initX;

            return nextPosition;
        }

        public void ReUse()
        {
            _isInit = false;
        }
    }
}
