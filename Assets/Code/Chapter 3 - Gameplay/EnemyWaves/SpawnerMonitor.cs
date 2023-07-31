using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class SpawnerMonitor : MonoBehaviour
    {
        private int _spawnerCount;
        private int _endedSpawners;
        private EventBus _eventBus;
        private PoolService _poolService;
        public void Initialize(int spawnerCount, EventBus eventBus, PoolService poolService)
        {
            _spawnerCount = spawnerCount;
            _eventBus = eventBus;
            _poolService = poolService;
            Subscribe();
        }
        private IEnumerator OnOneEndRoutine()
        {
            _endedSpawners++;

            if(_endedSpawners == _spawnerCount)
            {
                while (_poolService.HasAnyEnemies() == true)
                {
                    yield return null;
                }

                _eventBus.Invoke<AllEnemyDeath>(new AllEnemyDeath());
            }
        }

        private void OnOneSpawnEnd(ISignal signal)
        {
            StartCoroutine(OnOneEndRoutine());
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<SpawnEnd>(OnOneSpawnEnd);
        }

        private void OnDestroy()
        {
            _eventBus.Unsubscribe<SpawnEnd>(OnOneSpawnEnd);
        }
    }
}