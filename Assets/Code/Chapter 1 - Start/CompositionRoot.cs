using System;
using System.Collections.Generic;

namespace SpinalPlay
{
    internal class CompositionRoot
    {
        private Dictionary<Type, IService> _services;
        private const string PROGRESS_KEY = "SpinalPlay";
        private static bool _onceCreated = false;

        public CompositionRoot()
        {
            if (!_onceCreated)
            {
                _services = new Dictionary<Type, IService>();
                _onceCreated = true;
                return;
            }
            throw new Exception("Unacceptable attempt to re-create a CompositionRoot");
        }

        public void RegisterAllServices()
        {
            Register(new LoadingCurtain());
            Register(new AssetProviderService());
            Register(new SceneLoaderService());
            Register(new EventBus());
            Register(new AssetAddresses());
            Register(new SaveLoadProgressService(PROGRESS_KEY));
            Register(new DataTransferService(Get<SaveLoadProgressService>(), Get<EventBus>()));
            Register(new MusicService(Get<AssetProviderService>(), Get<AssetAddresses>(), Get<DataTransferService>()));
            Register(new SoundService(Get<EventBus>(), Get<AssetProviderService>(), Get<AssetAddresses>(), Get<DataTransferService>()));
            Register(new ConfigDirectoryService(Get<AssetProviderService>(), Get<AssetAddresses>()));
            Register(new PoolService(Get<EventBus>(), Get<AssetAddresses>(), Get<AssetProviderService>()));
            Register(new GameObjectFactory(Get<AssetProviderService>(), Get<AssetAddresses>(), Get<ConfigDirectoryService>(), Get<PoolService>(), Get<EventBus>(), Get<DataTransferService>(), Get<MusicService>()));
            Register(new UIFactory(Get<AssetProviderService>(), Get<AssetAddresses>(), Get<EventBus>(), Get<ConfigDirectoryService>(), Get<GameObjectFactory>(), Get<DataTransferService>()));
        }

        public GameStateMachine CreateGameStateMachine()
        {
            return new GameStateMachine(
                Get<DataTransferService>(),
                Get<UIFactory>(),
                Get<SceneLoaderService>(),
                Get<EventBus>(),
                Get<GameObjectFactory>(),
                Get<LoadingCurtain>(),
                Get<MusicService>());
        }

        private void Register<TService>(TService serviceInstance) where TService : class, IService
        {
            _services[typeof(TService)] = serviceInstance;
        }

        private TService Get<TService>() where TService : class, IService
        {
            return _services[typeof(TService)] as TService;
        }
    }
}