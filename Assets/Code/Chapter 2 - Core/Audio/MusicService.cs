using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace SpinalPlay
{
    public class MusicService : IService
    {
        private readonly AssetProviderService _assetProvider;
        private readonly AssetAddresses _addresses;

        private readonly SettingData _data;

        private AudioSource _musicPlayer;
        private const AssetType PREPARE_MUSIC_ADDRESS = AssetType.PrepareMusic;

        public MusicService(AssetProviderService assetProvider, AssetAddresses addresses, DataTransferService data) 
        {
            _assetProvider = assetProvider;
            _addresses = addresses;
            _data = data.Get<SettingData>();

            CreateMusicPlayer();
            Subscribe();
        }

        public async Task PlayMusicForPrepare()
        {
            await PlayMusic(PREPARE_MUSIC_ADDRESS);
        }

        public async Task PlayMusic(AssetType asset)
        {
            _musicPlayer.clip = await GetMusic(asset);
            _musicPlayer.Play();
        }

        private async Task<AudioClip> GetMusic(AssetType asset)
        {
            AssetReference address = _addresses.GetAssetReference(asset);
            AudioClip music = await _assetProvider.GetAsset<AudioClip>(address);
            return music;
        }

        private void CreateMusicPlayer()
        {
            GameObject musicObj = new GameObject("MusicPlayer");
            GameObject.DontDestroyOnLoad(musicObj);

            _musicPlayer = musicObj.AddComponent<AudioSource>();
            _musicPlayer.volume = _data.MusicVolume;
            _musicPlayer.loop = true;
        }

        private void UpdateMusicVolume(float volume)
        {
            _musicPlayer.volume = volume;
        }

        private void Subscribe()
        {
            _data.musicVolChange += UpdateMusicVolume;
        }
    }
}
