using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class GameState : IStateWithExit
    {
        private readonly GameObjectFactory _objectFactory;
        private readonly SceneLoaderService _sceneLoader;
        private readonly UIFactory _UIFactory;
        private readonly LoadingCurtain _curtain;
        private readonly DataTransferService _dataTransfer;

        private const string SCENE_NAME = "GameLoopScene";

        public GameState(GameObjectFactory objectFactory, SceneLoaderService sceneLoader, UIFactory uIFactory, LoadingCurtain curtain, DataTransferService dataTransfer) 
        {
            _objectFactory = objectFactory;
            _sceneLoader = sceneLoader;
            _UIFactory = uIFactory;
            _curtain = curtain;
            _dataTransfer = dataTransfer;
        }

        public void OnEnter()
        {
            _sceneLoader.LoadScene(SCENE_NAME, OnSceneLoaded);
        }

        public void OnExit()
        {
            _curtain.Show();
            _dataTransfer.SaveCurrent();
        }

        private async void OnSceneLoaded()
        {
            GameObject camera = await _UIFactory.CreateCamera();
            await CreateLevel();
            await CreateUIElements(camera);
            _curtain.Hide();
        }

        private async Task CreateLevel() 
        {
            await _objectFactory.CreateLevel();
            await _objectFactory.CreateShip();
        }

        private async Task CreateUIElements(GameObject camera)
        {
            GameObject overCanvas = await _UIFactory.CreateOverCanvas();

            await _UIFactory.CreateGameContainer(overCanvas.transform);

            GameObject resultWindow = await _UIFactory.CreateGameResultWindow(overCanvas.transform);
            resultWindow.SetActive(false);

            GameObject pausedWindow = await _UIFactory.CreatePauseWindow(overCanvas.transform);
            pausedWindow.SetActive(false);
        }
    }
}
