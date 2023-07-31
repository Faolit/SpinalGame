using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class LoadMenuState : IStateWithExit
    {
        private readonly UIFactory _uIfactory;
        private readonly SceneLoaderService _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly MusicService _music;
        private readonly GameObjectFactory _objectFactory;
        private readonly DataTransferService _dataTransfer;

        private const string SCENE_NAME = "MainMenuScene";

        public LoadMenuState(UIFactory uIFuctory, SceneLoaderService sceneLoader, LoadingCurtain curtain, MusicService music, GameObjectFactory objectFactory, DataTransferService dataTransfer)
        {
            _uIfactory = uIFuctory;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _music = music;
            _objectFactory = objectFactory;
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
            GameObject camera = await _uIfactory.CreateCamera();

            await CreateUIElements(camera);

            await _music.PlayMusicForPrepare();

            await _objectFactory.CreateCloudSpavner(new Vector2(14, 1), 3, 4);

            _curtain.Hide();
        }

        private async Task CreateUIElements(GameObject camera)
        {
            GameObject rootCanvas = await _uIfactory.CreateRootCanvas(camera.GetComponent<Camera>(), -10);

            await _uIfactory.CreateMainMenuContainer(rootCanvas.transform);

            GameObject settingWindow = await _uIfactory.CreateSettingWindow(rootCanvas.transform);
            settingWindow.SetActive(false);

            GameObject warningWindow = await _uIfactory.CreateWarningWindow(rootCanvas.transform);
            warningWindow.SetActive(false);
        }

    }
}