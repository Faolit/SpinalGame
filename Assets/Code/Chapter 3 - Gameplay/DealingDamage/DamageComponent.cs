using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class DamageComponent : MonoBehaviour
    {
        public UnitTag UnitTag { get; private set; }
        public int Damage { get; private set; }
        private bool _onCollsionnDestroyed;
        private EventBus _eventBus;

        public void Initialize(int damage, UnitTag damageTag, bool onCollisionDestroyed, EventBus eventBus)
        {
            Damage = damage;
            UnitTag = damageTag;
            _onCollsionnDestroyed = onCollisionDestroyed;
            _eventBus = eventBus;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {
            HealthComponent healthComponent = collision.gameObject.GetComponent<HealthComponent>();
            if (healthComponent == null)
            {
                return;
            }
            if (healthComponent.UnitTag != UnitTag && _onCollsionnDestroyed)
            {
                _eventBus.Invoke<ProjectileDestroy>(new ProjectileDestroy(gameObject));
            }
        }
    }
}