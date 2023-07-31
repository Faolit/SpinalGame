using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class AutonomAttackComponent : MonoBehaviour
    {
        [SerializeField] private WeaponHolderPoint _weaponHolder;
        public WeaponHolderPoint WeaponHolder { get { return _weaponHolder; } }

        public void Initialize(GameObject weapon)
        { 
            _weaponHolder.SetGun(weapon);
        }

        private void OnEnable()
        {
            if (_weaponHolder.isGunSet)
            {
                StartCoroutine("Attack");
            }
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator Attack()
        {
            while(true)
            {
                yield return WeaponHolder?.Attack();
            }
        }
    }
}