using System.Collections.Generic;
using UnityEngine;

namespace SpinalPlay
{
    public static class CameraCalc
    {
        private static Camera _camera => Camera.main;
        private static float x = 0;
        private static float y = 0;
        public static float GetCameraHeight()
        {
            return (float)(_camera.orthographicSize * 2.0);
        }

        public static float GetCameraWeidth()
        {
            return (GetCameraHeight() * Screen.width / Screen.height);
        }

        public static Vector2 GetLeftCenter(float offset)
        {
            x = _camera.transform.position.x - offset - GetCameraWeidth() / 2;
            y = _camera.transform.position.y;
            return new Vector2(x, y);
        }

        public static Vector2 GetRightCenter(float offset)
        {
            x = _camera.transform.position.x + offset + GetCameraWeidth() / 2;
            y = _camera.transform.position.y;
            return new Vector2(x, y);
        }

        public static Vector2 GetTopCenter(float offset)
        {
            x = _camera.transform.position.x;
            y = _camera.transform.position.y + offset + GetCameraHeight() / 2;
            return new Vector2(x, y);
        }

        public static Vector2 GetDownCenter(float offset)
        {
            x = _camera.transform.position.x;
            y = _camera.transform.position.y - offset - GetCameraHeight() / 2;
            return new Vector2(x, y);
        }

        public static List<BoundData> GetCameraBoundsData(float offset)
        {
            List<BoundData> data = new List<BoundData>
            {
                new BoundData(1, GetCameraHeight() + offset * 2, GetLeftCenter(offset)),
                new BoundData(1, GetCameraHeight() + offset * 2, GetRightCenter(offset)),
                new BoundData(GetCameraWeidth() + offset * 2, 1, GetTopCenter(offset)),
                new BoundData(GetCameraWeidth() + offset * 2, 1, GetDownCenter(offset))
            };

            return data;    
        }
    }
}
