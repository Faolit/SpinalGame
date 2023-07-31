using System;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace SpinalPlay
{
    public class AssetAddresses : IService
    {
        private Dictionary<AssetType, AssetReference> _references;

        public AssetAddresses() 
        {
            _references = new Dictionary<AssetType, AssetReference>();
            InitializeAllReferences();
        }

        public string GetAssetLabel(AssetLabels label)
        {
            return label.ToString();
        }

        public AssetReference GetAssetReference(AssetType type)
        {
            return _references[type];
        }

        private void AddAssetReference(AssetType key)
        {
            _references[key] = new AssetReference(key.ToString());
        }

        private void InitializeAllReferences()
        {
            Array assetTypeValues = Enum.GetValues(typeof(AssetType));
            foreach (AssetType reference in assetTypeValues) 
            {
                AddAssetReference(reference);
            }
        }
    }
}