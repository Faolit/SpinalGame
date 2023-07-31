using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class AutonomMoveComponent : MonoBehaviour
    {
        [SerializeField] Rigidbody2D rb;

        private MoveConfig _config;
        private ITrajectory _bulletSetup;
        private Vector2 _direction;

        public void Initialize(MoveConfig config)
        {
            TrajectoryFactory factory = new TrajectoryFactory();
            _bulletSetup = factory.CreateTrajectoryCalc(config.trajectoryType);
            _config = config;
        }

        public void SetNewWay(Vector2 startPosition, Vector2 startDirection)
        {
            transform.position = startPosition;
            _direction = startDirection;

            if(_bulletSetup is IReUsedTrajectory)
            {
                ((IReUsedTrajectory)_bulletSetup).ReUse();
            }
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            rb.MovePosition(_bulletSetup.CalculateMove(transform.position, _direction, _config));
        }
    }
}