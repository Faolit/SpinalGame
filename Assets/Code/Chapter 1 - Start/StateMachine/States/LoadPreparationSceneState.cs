using System.Threading.Tasks;
using UnityEngine;

namespace SpinalPlay
{
    public class LoadPreparationSceneState : IStateWithExit
    {
        private readonly UIFactory _UIfactory;
        private readonly SceneLoaderService _sceneLoader;
        private readonly LoadingCurtain _curtain;
        private readonly MusicService _music;
        private readonly DataTransferService _dataTransfer;

        private const string SCENE_NAME = "PreparationScene";

        public LoadPreparationSceneState(UIFactory uIFactory, SceneLoaderService sceneLoader, LoadingCurtain curtain, MusicService music, DataTransferService dataTransfer)
        {
            _UIfactory = uIFactory;
            _sceneLoader = sceneLoader;
            _curtain = curtain;
            _music = music;
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
            GameObject camera = await _UIfactory.CreateCamera();

            await CreateUIElements(camera);

            _curtain.Hide();

            await _music.PlayMusicForPrepare();
        }

        private async Task CreateUIElements(GameObject camera)
        {
            GameObject rootCanvas = await _UIfactory.CreateRootCanvas(camera.GetComponent<Camera>(), -10);
            GameObject overCanvas = await _UIfactory.CreateOverCanvas();

            await _UIfactory.CreatePreparationSceneContainer(rootCanvas.transform);

            GameObject shipShopWindow = await _UIfactory.CreateShipShopWindow(rootCanvas);
            shipShopWindow.SetActive(false);

            GameObject weaponShopWindow = await _UIfactory.CreateWeaponShopWindow(rootCanvas);
            weaponShopWindow.SetActive(false);

            GameObject notificWindow = await _UIfactory.CreateNotificationWindow(overCanvas.transform);
            notificWindow.SetActive(false);
        }
    }
}