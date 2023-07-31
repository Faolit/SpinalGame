using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpinalPlay
{
    public class AttackComponent : MonoBehaviour
    {
        [SerializeField] private WeaponHolderPoint _weaponHolder;

        public WeaponHolderPoint WeaponHolder { get { return _weaponHolder; } }

        private InputActionAsset _input;

        private bool isAttack = false;
        private bool isAtkEnd = true;
        private bool isAtkInvoke = false;

        public void Initialize(InputActionAsset input, GameObject weapon)
        {
            _input = input;

            _input.FindActionMap("Player").FindAction("Attack").performed += OnAtkPressed;
            _input.FindActionMap("Player").FindAction("Attack").canceled += OnAtkCanceled;
            _input.FindActionMap("Player").Enable();

            _weaponHolder.SetGun(weapon);
        }

        private void OnAtkPressed(InputAction.CallbackContext context)
        {
            if(isAtkEnd)
            {
                isAttack = true;
                StartCoroutine("Attack");
            }
            else
            {
                isAtkInvoke = true;
            }
        }

        private void OnAtkCanceled(InputAction.CallbackContext context)
        {
            if(isAtkInvoke) 
            {
                return;
            }
            else
            {
                isAttack = false;
            }
        }

        private IEnumerator Attack()
        {
            isAtkInvoke = false;
            isAtkEnd = false;
            while(isAttack)
            {
                yield return WeaponHolder?.Attack();
            }
            isAtkEnd = true;
        }

        private void OnDestroy()
        {
            if(_input != null)
            {
                _input.FindActionMap("Player").FindAction("Attack").performed -= OnAtkPressed;
                _input.FindActionMap("Player").FindAction("Attack").canceled -= OnAtkCanceled;
            }
        }
    }
}