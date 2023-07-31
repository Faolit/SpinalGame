using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class CloudSpavner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _clouds;
        [SerializeField] private MoveConfig _moveConfig;

        PoolService _poolService;

        private float _maxOffsetY;
        private float _spavnDelay;

        public void Initialize(PoolService poolService, Vector2 StartPos, float maxOffetY, float spavnDelay)
        {
            _poolService = poolService;

            _maxOffsetY = maxOffetY;
            _spavnDelay = spavnDelay;

            transform.position = StartPos;

            StartCoroutine("StartSpavning");
        }

        private IEnumerator StartSpavning()
        {
            while(true)
            {
                SpavnRandomCloud();
                yield return new WaitForSeconds(_spavnDelay);
            }
        }

        private void SpavnRandomCloud()
        {
            GameObject cloud = GetRandomCloud();

            float offsetY = Random.Range(-_maxOffsetY, _maxOffsetY);
            Vector2 pos = new Vector2(transform.position.x, transform.position.y + offsetY);

            AutonomMoveComponent move = cloud.GetComponent<AutonomMoveComponent>();
            move.Initialize(_moveConfig);
            move.SetNewWay(pos, Vector2.left);

            cloud.SetActive(true);
        }

        private GameObject GetRandomCloud()
        {
            GameObject pref = _clouds[Random.Range(0, _clouds.Length)];
            return _poolService.GetCloud(pref, _moveConfig);
        }
    }
}