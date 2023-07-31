using System.Collections;
using UnityEngine;

namespace SpinalPlay
{
    public class WeaponHolderPoint : MonoBehaviour
    {
        public bool isGunSet = false;
        private WeaponComponent _gun; 

        public IEnumerator Attack()
        {
            yield return StartCoroutine(_gun.Attack());
        }
        public void SetGun(GameObject gun)
        {
            if(gun.GetComponent<WeaponComponent>() != null)
            {
                gun.transform.SetParent(gameObject.transform);
                gun.transform.localPosition = Vector3.zero;
                _gun = gun.GetComponent<WeaponComponent>();
                isGunSet = true;
            }
        }
        public Vector2 GetHolderPosition()
        {
            return gameObject.transform.position;
        }
    }
}