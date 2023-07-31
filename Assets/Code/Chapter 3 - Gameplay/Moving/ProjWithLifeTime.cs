using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class ProjWithLifeTime : MonoBehaviour
    {
        private float _lifeTime;

        public void Initialize(float lifeTime)
        {
            _lifeTime = lifeTime;
        }

        private void OnEnable()
        {
            StartCoroutine("DeathClock");
        }

        public IEnumerator DeathClock()
        {
            yield return new WaitForSeconds(_lifeTime);
            gameObject.SetActive(false);
        }
    }
}