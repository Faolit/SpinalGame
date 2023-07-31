using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpinalPlay
{
    public class SoundService : IService
    {
        private readonly EventBus _eventBus;
        private readonly AssetProviderService _assetProvider;
        private readonly AssetAddresses _addresses;

        private readonly Transform _soundParent;
        private SimpleGameObjectPool _pool;
        private SettingData _settingData;
        private float _volume;

        public SoundService(EventBus eventBus, AssetProviderService assetProvider, AssetAddresses addresses, DataTransferService dataTransfer) 
        {
            _eventBus = eventBus;
            _assetProvider = assetProvider;
            _addresses = addresses;

            _soundParent = new GameObject("SoundService").transform;
            _pool = new SimpleGameObjectPool(10, CreateSoundSource);
            _settingData = dataTransfer.Get<SettingData>(); 

            Subscribe();
        }

        private GameObject CreateSoundSource()
        {
            GameObject sourceObj = new GameObject("SoundSource");
            sourceObj.transform.SetParent(_soundParent);

            AudioSource sourceComp = sourceObj.AddComponent<AudioSource>();
            sourceComp.loop = false;
            sourceComp.playOnAwake = false;

            sourceObj.AddComponent<PooledAudioSource>();
            
            return sourceObj;
        }

        private void InvokeSound(InvokeSound signal)
        {
            AutonomCoroutineRunner.StartRoutine(InvokeSoundRoutine(signal));
        }

        private IEnumerator InvokeSoundRoutine(InvokeSound signal)
        {
            GameObject sourceObj = _pool.Get();

            PooledAudioSource pooledSourceComp = sourceObj.GetComponent<PooledAudioSource>();

            Task<AudioClip> task = GetSound(signal.sound);
            yield return new WaitUntil(() => task.IsCompleted);

            sourceObj.SetActive(true);
            pooledSourceComp.StartSound(task.Result, _volume);
        }

        private async Task<AudioClip> GetSound(AssetType asset)
        {
            AssetReference address = _addresses.GetAssetReference(asset);
            AudioClip sound = await _assetProvider.GetAsset<AudioClip>(address);
            return sound;
        }

        private void UpdateSoundVolume(float volume)
        {
            _volume = volume;
        }

        private void Subscribe()
        {
            _eventBus.Subscribe<InvokeSound>(InvokeSound);
            _settingData.soundVolChange += UpdateSoundVolume;
        }
    }
}
