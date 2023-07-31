using UnityEngine;

namespace SpinalPlay
{
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] GameObject particleObj;
        [SerializeField] HealthComponent health;

        private void StartEffect()
        {
            GameObject obj = Instantiate(particleObj, transform.position,Quaternion.identity);
            ParticleSystem particle = obj.GetComponent<ParticleSystem>();
            if (particle != null)
            {
                particle.Play();
            }
        }

        private void OnEnable()
        {
            health.onDeath += StartEffect;
        }

        private void OnDisable()
        {
            health.onDeath -= StartEffect;
        }
    }
}