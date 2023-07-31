using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpinalPlay
{
    public class MovePlayerComponent : MonoBehaviour
    {
        private ShipConfig _config;
        private InputActionAsset _input;

        private Rigidbody2D _rb;
        private PolygonCollider2D _col;

        private InputAction _moveAction;
        private Vector2 _moveValue;

        private int countOfRays = 6;
        private float RayDistance = 0.1f;
        private float bufferRayDist = 0.1f;
        private LayerMask layerMask = 1 << 7;

        private bool downCol, leftCol, rightCol, topCol;
        private RayRange downSide, rightSide, leftSide, topSide;

        public void Initialize(ShipConfig config, InputActionAsset input)
        {
            _config = config;
            _input = input;
            
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<PolygonCollider2D>();

            _moveAction = _input.FindActionMap("Player").FindAction("Moving");
            _input.FindActionMap("Player").Enable();
        }

        private void FixedUpdate()
        {
            if(_rb != null)
            {
                Move();
            }
        }

        private void Move()
        {
            _moveValue = _moveAction.ReadValue<Vector2>();

            CalculateCollisionsEffect();

            _rb.MovePosition((Vector2)transform.position + _moveValue * _config.moveConfig.speed * Time.fixedDeltaTime);
        }

        private void OnDestroy()
        {
            if( _input != null )
            {
                _input.FindActionMap("Player").Disable();
            }
        }

        private void CalculateCollisionsEffect()
        {
            DefineRayRange();
            CheckSidesCollision();

            if (downCol)
            {
                _moveValue.y = Mathf.Clamp(_moveValue.y, 0, 1);
            }
            if (leftCol)
            {
                _moveValue.x = Mathf.Clamp(_moveValue.x, 0, 1);
            }
            if (rightCol)
            {
                _moveValue.x = Mathf.Clamp(_moveValue.x, -1, 0);
            }
            if (topCol)
            {
                _moveValue.y = Mathf.Clamp(_moveValue.y, -1, 0);
            }
        }
        private void CheckSidesCollision()
        {
            downCol = CheckSideCollision(downSide, RayDistance, Vector2.down);
            topCol = CheckSideCollision(topSide, RayDistance, Vector2.up);
            rightCol = CheckSideCollision(rightSide, RayDistance, Vector2.right);
            leftCol = CheckSideCollision(leftSide, RayDistance, Vector2.left);
        }
        private bool CheckSideCollision(RayRange range, float distance, Vector2 dir)
        {
            foreach (var origin in GetRayOrigin(range))
            {
                Debug.DrawRay(origin, dir, Color.red, distance);
                if (Physics2D.Raycast(origin, dir, distance, layerMask))
                {
                    return true;
                }
            }
            return false;
        }
        private IEnumerable<Vector2> GetRayOrigin(RayRange range)
        {
            for (int i = 0; i < countOfRays; i++)
            {
                float t = i / (countOfRays - 1);
                yield return Vector2.Lerp(range.start, range.end, t);
            }
        }
        private void DefineRayRange()
        {
            Bounds b = _col.bounds;
            downSide = new RayRange(b.min.x, b.min.y - bufferRayDist, b.max.x, b.min.y - bufferRayDist);
            rightSide = new RayRange(b.max.x + bufferRayDist, b.min.y, b.max.x + bufferRayDist, b.max.y);
            leftSide = new RayRange(b.min.x - bufferRayDist, b.min.y, b.min.x - bufferRayDist, b.max.y);
            topSide = new RayRange(b.min.x, b.max.y + bufferRayDist, b.max.x, b.max.y + bufferRayDist);
        }
    }
}