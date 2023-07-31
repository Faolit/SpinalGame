namespace SpinalPlay
{
    public class RepeatingGameState : IState
    {
        private readonly EventBus _eventBus;
        private readonly SceneLoaderService _sceneLoader;

        private const string SCENE_NAME = "RepeatingScene";

        public RepeatingGameState(EventBus eventBus, SceneLoaderService sceneLoader)
        {
            _eventBus = eventBus;
            _sceneLoader = sceneLoader;
        }

        public void OnEnter()
        {
            _sceneLoader.LoadScene(SCENE_NAME, OnSceneLoaded);
        }

        private void OnSceneLoaded()
        {
            _eventBus.Invoke<EnterToGameLoop>(new EnterToGameLoop());
        }
    }
}
