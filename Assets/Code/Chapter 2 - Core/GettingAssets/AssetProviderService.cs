using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace SpinalPlay
{
    public class AssetProviderService : IService
    {
        private Dictionary<string, AsyncOperationHandle> _cachedHandles;

        public AssetProviderService()
        {
            _cachedHandles = new Dictionary<string, AsyncOperationHandle>();
        }

        public async Task<IList<T>> GetAssets<T>(string label) where T : ConfigBase
        {
            IList<IResourceLocation> locations = await Addressables.LoadResourceLocationsAsync(label).Task;
            List<Task<T>> tasks = new List<Task<T>>();
            List<T> result = new List<T>();

            foreach (IResourceLocation location in locations)
            {
                tasks.Add(GetAsset<T>(location));
            }

            await Task.WhenAll(tasks);

            foreach(Task<T> task in tasks)
            {
                result.Add(task.Result);
            }

            return result;
        }

        public async Task<T> GetAsset<T>(AssetReference reference) where T : class
        {
            string strRef = reference.ToString();

            if (_cachedHandles.ContainsKey(strRef))
            {
                if (_cachedHandles[strRef].Status == AsyncOperationStatus.Succeeded)
                {
                    return _cachedHandles[strRef].Result as T;
                }
                return await _cachedHandles[strRef].Task as T;
            }
            return await CreateHandle<T>(reference);
        }

        public async Task<T> GetAsset<T>(IResourceLocation location) where T : class
        {
            string strLoc = location.PrimaryKey;

            if (_cachedHandles.ContainsKey(strLoc))
            {
                if (_cachedHandles[strLoc].Status == AsyncOperationStatus.Succeeded)
                {
                    return _cachedHandles[strLoc].Result as T;
                }
                return await _cachedHandles[strLoc].Task as T;
            }
            return await CreateHandle<T>(location);
        }

        private async Task<T> CreateHandle<T>(AssetReference reference) where T : class
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(reference);
            _cachedHandles[reference.ToString()] = handle;
           
            return await handle.Task as T;
        }
        private async Task<T> CreateHandle<T>(IResourceLocation location) where T : class
        {
            AsyncOperationHandle handle = Addressables.LoadAssetAsync<T>(location);
            _cachedHandles[location.PrimaryKey] = handle;
            return await handle.Task as T;
        }
    }
}