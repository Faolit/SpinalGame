using UnityEngine;

namespace SpinalPlay
{
    public class HealthComponent : MonoBehaviour
    {
        public event deathDelegate onDeath;
        public delegate void deathDelegate();
        public UnitTag UnitTag { get; private set; }
        public int Health { get { return _currentHp; } private set {; } }

        private int _maxHp;
        private int _currentHp;
        private EventBus _eventBus;

        public void Inititalize(int maxHp, EventBus eventBus, UnitTag tag)
        {
            _maxHp = maxHp;
            _currentHp = maxHp;
            _eventBus = eventBus;
            UnitTag = tag;
        }

        public void PrepareToUse()
        {
            _currentHp = _maxHp;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            DamageComponent damageComponent = collision.gameObject.GetComponent<DamageComponent>();
            if(damageComponent == null)
            {
                return;
            }
            if(damageComponent.UnitTag != UnitTag)
            {
                TakeDamage(damageComponent.Damage);
            }
        }

        private void TakeDamage(int damage)
        {
            if(UnitTag == UnitTag.Player)
            {
                _eventBus.Invoke<PlayerDamaged>(new PlayerDamaged(damage));
            }

            _currentHp -= damage;

            if(_currentHp <= 0)
            {
                Death();
            }
        }

        private void Death()
        {
            switch (UnitTag)
            {
                case UnitTag.None:
                    break;

                case UnitTag.Player:
                    _eventBus.Invoke<PlayerDead>(new PlayerDead());
                    onDeath?.Invoke();
                    GameObject.Destroy(gameObject);
                    break;

                case UnitTag.Enemy:
                    _eventBus.Invoke<EnemyDead>(new EnemyDead(gameObject));
                    onDeath?.Invoke();
                    break;

                default:
                    break;
            }
        }
    }
}