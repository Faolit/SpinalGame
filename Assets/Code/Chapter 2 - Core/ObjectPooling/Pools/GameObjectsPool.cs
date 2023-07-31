using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class GameObjectsPool<Conf>
    {
        private Func<Conf, Task<GameObject>> _poolFunc;
        private Conf _config;
        protected int _initialSize;
        protected List<PooledObject> _pools;

        public GameObjectsPool(int initialSize, Func<Conf, Task<GameObject>> factoryFunc, Conf config)
        {
            _initialSize = initialSize;
            _poolFunc = factoryFunc;
            _pools = new List<PooledObject>();
            _config = config;

            AutonomCoroutineRunner.StartRoutine(CreateInitialObjects());
        }

        public async Task<GameObject> Get()
        {
            PooledObject obj = _pools.FirstOrDefault(x => !x.inUsed);

            if (obj == null)
            {
                obj = await Create();
                _pools.Add(obj);
            }

            obj.inUsed = true;

            return obj.gameObject;
        }

        public bool HasAnyActive()
        {
            if (_pools.FirstOrDefault(x => x.inUsed) != null) 
            {
                return true;
            }
            return false;
        }

        protected IEnumerator CreateInitialObjects()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                Task<PooledObject> task = Create();
                yield return new WaitUntil(() => task.IsCompleted);
                _pools.Add(task.Result);
            }
        }

        protected async Task<PooledObject> Create()
        {
            GameObject obj = await _poolFunc.Invoke(_config);

            if (obj.activeSelf) obj.SetActive(false);

            return obj.AddComponent<PooledObject>();
        }
    }
}