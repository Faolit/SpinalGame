using UnityEngine;

namespace SpinalPlay
{
    public class PooledObject : MonoBehaviour
    {
        public bool inUsed = false;

        private void OnDisable()
        {
            inUsed = false;
        }
    }
}