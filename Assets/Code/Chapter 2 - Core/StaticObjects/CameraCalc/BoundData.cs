using UnityEngine;

namespace SpinalPlay
{
    public class BoundData
    {
        public BoundData(float sizeX, float sizeY, Vector2 pos)
        {
            this.sizeX = sizeX;
            this.sizeY = sizeY;
            this.pos = pos;
        }
        public float sizeX;
        public float sizeY;
        public Vector2 pos;
    }
}
