using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class ConfigDirectoryService : IService
    {
        private Dictionary<Type, Dictionary<int, ConfigBase>> _allConfigs;

        private AssetAddresses _addresses;
        private AssetProviderService _assets;
        public ConfigDirectoryService(AssetProviderService assets, AssetAddresses addresses) 
        {
            _addresses = addresses;
            _assets = assets;
        }
        public async Task<T[]> GetConfigs<T>() where T : ConfigBase
        {
            if (_allConfigs == null)
            {
                await InitializeConfigs();
            }
            if (!_allConfigs.ContainsKey(typeof(T)))
            {
                throw new Exception($"Config dictionary with {typeof(T)} does not exist");
            }
            if (_allConfigs[typeof(T)] == null)
            {
                throw new Exception($"Could not find configurations of {typeof(T)} type");
            }
            List<T> configs = new List<T>();
            foreach(ConfigBase config in _allConfigs[typeof(T)].Values)
            {
                configs.Add((T)config);
            }
            return configs.ToArray();
        }
        public async Task<T> GetConfig<T>(int id) where T : ConfigBase
        {
            if(_allConfigs == null)
            {
                await InitializeConfigs();
            }
            if(!_allConfigs.ContainsKey(typeof(T)))
            {
                throw new Exception($"Config dictionary with {typeof(T)} does not exist");
            }
            if (_allConfigs[typeof(T)] == null)
            {
                throw new Exception($"Could not find configurations of {typeof(T)} type");
            }
            if (!_allConfigs[typeof(T)].ContainsKey(id))
            {
                throw new Exception($"Config {typeof(T)} with id {id} does not exist");
            }
            return (T)_allConfigs[typeof(T)][id];

        }
        private async Task InitializeConfigs()
        {
            _allConfigs = new Dictionary<Type, Dictionary<int, ConfigBase>>();
            await AddToAllConf<ShipConfig>(AssetLabels.ShipConfigs);
            await AddToAllConf<WeaponConfig>(AssetLabels.WeaponConfigs);
            await AddToAllConf<LevelConfig>(AssetLabels.LevelConfigs);
        }
        private async Task AddToAllConf<T>(AssetLabels label) where T : ConfigBase
        {
            Type key = typeof(T);

            Dictionary<int, T> dict = await GetConfigDict<T>(label);
            Dictionary<int, ConfigBase> value = new Dictionary<int, ConfigBase>();

            foreach(var kvp in dict) 
            {
                value.Add(kvp.Key, kvp.Value);
            }

            _allConfigs.Add(key, value);
        }
        private async Task<Dictionary<int, T>> GetConfigDict<T>(AssetLabels label) where T : ConfigBase
        {
            var task = await GetConfigList<T>(label);
            var result = ConfigListToDict<T>(task);
            return result;
        }
        private Dictionary<int, T> ConfigListToDict<T>(List<T> configs) where T : ConfigBase
        {
            Dictionary<int, T> dict = new Dictionary<int, T>();

            if(configs == null) 
            {
                Debug.Log($"Configs {typeof(T)} is null");
                return dict;
            }     
            if(configs.Count == 0)
            {
                Debug.Log($"Configs {typeof(T)} dont exist");
                return dict;
            }
            foreach(var config in configs)
            {
                int key = config.ID;
                dict.Add(key, config);
            }

            return dict;
        }
        private async Task<List<T>> GetConfigList<T>(AssetLabels label) where T : ConfigBase
        {
            var task = await _assets.GetAssets<T>(_addresses.GetAssetLabel(label));
            return (List<T>)task;
        }
    }
}
