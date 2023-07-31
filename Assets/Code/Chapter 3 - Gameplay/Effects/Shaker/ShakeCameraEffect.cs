using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpinalPlay
{
    public class ShakeCameraEffect : MonoBehaviour
    {
        private EventBus _eventBus;
        private List<ShakeSetting> _shakeSets;

        public void Initialize(EventBus eventBus)
        {
            _eventBus = eventBus;
            InitShakeSettings();
            Subscribe();
        }

        private IEnumerator StartShake(ShakeType shakeType)
        {
            ShakeSetting shakeSetting = _shakeSets.FindLast(x => x.shakeType == shakeType);
            if (shakeSetting != null)
            {
                float timeLost = shakeSetting.duration;
                Vector3 startPos = gameObject.transform.position;

                while (timeLost >= 0)
                {
                    Shake(shakeSetting.intense, startPos);
                    yield return new WaitForSeconds(shakeSetting.delay);
                    timeLost -= shakeSetting.delay;
                }

                gameObject.transform.position = startPos;
            }
        }

        private void Shake(float intense, Vector3 startPos)
        {
            Vector3 offset = new Vector3(Random.Range(-intense, intense), Random.Range(-intense, intense));
            Vector3 newPos = startPos + offset;
            gameObject.transform.position = newPos;
        }

        private void InitShakeSettings()
        {
            _shakeSets = new List<ShakeSetting>
            {
                new ShakeSetting(ShakeType.Punch, 0.5f, 0f, 1f),
                new ShakeSetting(ShakeType.Tiny, 0.05f, 0.8f, 0.05f),
                new ShakeSetting(ShakeType.Normal, 0.1f, 1f, 0.1f),
                new ShakeSetting(ShakeType.Colossal, 0.25f, 3f, 0.1f)
            };
        }

        private void OnShakeEvent(ISignal signal)
        {
            StartCoroutine(StartShake(ShakeType.Normal));
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<EnemyDead>(OnShakeEvent);
        }

        private void OnDisable()
        {
            _eventBus.Unsubscribe<EnemyDead>(OnShakeEvent);
        }
    }
}