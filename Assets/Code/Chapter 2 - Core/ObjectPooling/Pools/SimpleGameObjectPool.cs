using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using UnityEngine;
using System.Linq;
using System.Collections;

namespace SpinalPlay
{
    public class SimpleGameObjectPool
    {
        private Func<GameObject> _poolFunc;
        protected int _initialSize;
        protected List<PooledObject> _pools;

        public SimpleGameObjectPool(int initialSize, Func<GameObject> factoryFunc)
        {
            _initialSize = initialSize;
            _poolFunc = factoryFunc;
            _pools = new List<PooledObject>();
            CreateInitialObjects();
        }

        public GameObject Get()
        {
            PooledObject obj = _pools.FirstOrDefault(x => !x.inUsed);

            if (obj == null)
            {
                obj = Create();
                _pools.Add(obj);
            }

            obj.inUsed = true;

            return obj.gameObject;
        }

        private void CreateInitialObjects()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                _pools.Add(Create());
            }
        }


        private PooledObject Create()
        {
            GameObject obj = _poolFunc.Invoke();

            if (obj.activeSelf) obj.SetActive(false);

            return obj.AddComponent<PooledObject>();
        }
    }
}
