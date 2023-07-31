using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class EnemySpawnerComponent : MonoBehaviour
    {
        private SpavnerConfig _config;
        private PoolService _poolService;
        private WaveConfig[] _waveConfigs;
        private EventBus _bus;

        public void Initialize(SpavnerConfig config, PoolService poolService, EventBus eventBus)
        {
            _config = config;
            _poolService = poolService;
            _bus = eventBus;

            _waveConfigs = _config.waveConfigs;
            SetSpavnerPos();
        }

        public void InvokeSpawn()
        {
            StartCoroutine(StartSpaun());
        }

        private IEnumerator StartSpaun()
        {
            foreach (WaveConfig wave in _waveConfigs)
            {
                yield return StartCoroutine(StartWave(wave));

                yield return new WaitForSeconds(_config.waveDelay);
            }

            _bus.Invoke<SpawnEnd>(new SpawnEnd());
        }

        private IEnumerator StartWave(WaveConfig waveConfig)
        {
            foreach (EnemyConfig enemyConfig in waveConfig.enemyConfigs)
            {
                Task<GameObject> task = _poolService.GetEnemy(enemyConfig);
                yield return new WaitUntil(() => task.IsCompleted);
                GameObject enemy = task.Result;

                AutonomMoveComponent enemyMove = enemy.GetComponent<AutonomMoveComponent>();
                Vector2 newPos = new Vector2(transform.position.x + Random.Range(-_config.randomPos, _config.randomPos), transform.position.y);
                enemyMove.SetNewWay(newPos, Vector2.down);

                HealthComponent health = enemy.GetComponent<HealthComponent>();
                health.PrepareToUse();

                enemy.SetActive(true);

                yield return new WaitForSeconds(waveConfig.enemyDelay);
            }
        }

        private void SetSpavnerPos()
        {
            if (CameraCalc.GetLeftCenter(0).x > _config.spavnerPosition.x)
            {
                transform.position = new Vector2(CameraCalc.GetLeftCenter(0).x, _config.spavnerPosition.y);
                return;
            }
            if (CameraCalc.GetRightCenter(0).x < _config.spavnerPosition.x)
            {
                transform.position = new Vector2(CameraCalc.GetRightCenter(0).x, _config.spavnerPosition.y);
                return;
            }
            transform.position = _config.spavnerPosition;
        }
    }
}