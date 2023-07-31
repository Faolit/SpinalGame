using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpinalPlay
{
    public abstract class FactoryBase
    {
        protected AssetProviderService _assets;
        protected AssetAddresses _references;

        protected async Task<GameObject> AsyncInstantiate(AssetType type, Vector3 pos)
        {
            GameObject pref = await GetPrefab(type);
            GameObject obj = GameObject.Instantiate(pref, pos, Quaternion.identity);
            return obj;
        }

        protected async Task<GameObject> AsyncInstantiate(AssetType type)
        {
            GameObject pref = await GetPrefab(type);
            GameObject obj = GameObject.Instantiate(pref);
            return obj;
        }

        protected async Task<GameObject> AsyncInstantiate(AssetType type, Transform parent)
        {
            GameObject pref = await GetPrefab(type);
            GameObject obj = GameObject.Instantiate(pref, parent);
            return obj;
        }

        private async Task<GameObject> GetPrefab(AssetType type)
        {
            AssetReference reference = _references.GetAssetReference(type);
            return await _assets.GetAsset<GameObject>(reference);
        } 

    }
}