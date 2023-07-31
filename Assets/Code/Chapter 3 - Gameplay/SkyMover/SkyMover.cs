using UnityEngine;

namespace SpinalPlay
{
    public class SkyMover : MonoBehaviour
    {
        private Camera _camera;
        private SkyObject[] _skies;

        private float _cameraHeight;
        private float _cameraWidth;

        public void Initialize(SkyObject[] skies)
        {
            _camera = Camera.main;

            _cameraHeight = CameraCalc.GetCameraHeight();
            _cameraWidth = CameraCalc.GetCameraWeidth();

            _skies = skies;

            foreach (var sky in _skies)
            {
                Resize(sky);
            }
        }

        private void FixedUpdate()
        {
            foreach (var sky in _skies)
            {
                Move(sky);
            }
        }

        private void Resize(SkyObject sky)
        {
            sky.sprite.size = new Vector2(_cameraWidth + sky.sourceSizeXScaled * 2, _cameraHeight + sky.sourceSizeYScaled * 2);
            sky.lastPositionY = _camera.transform.position.y;
        }

        private void Move(SkyObject sky)
        {
            sky.lastPositionY -= sky.speed * Time.fixedDeltaTime;
            sky.lastPositionY = Mathf.Repeat(sky.lastPositionY, sky.trueSizeY);
            sky.sprite.transform.position = new Vector2(0, sky.lastPositionY);
        }
    }
}