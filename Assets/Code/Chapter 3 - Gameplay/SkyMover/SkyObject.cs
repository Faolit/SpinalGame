using UnityEngine;

namespace SpinalPlay
{
    public class SkyObject
    {
        public GameObject obj;
        public SpriteRenderer sprite;

        public float lastPositionY;

        public float speed;

        public float sourceSizeYScaled;
        public float sourceSizeXScaled;

        public float sourceSizeY;
        public float sourceSizeX;

        public float trueSizeX;
        public float trueSizeY;

        public SkyObject(GameObject obj, float speed)
        {
            this.obj = obj;
            this.speed = speed;
            sprite = obj.GetComponent<SpriteRenderer>();
            lastPositionY = 0;

            sourceSizeX = sprite.size.x;
            sourceSizeY = sprite.size.y;

            sourceSizeYScaled = sprite.size.y / obj.transform.localScale.y;
            sourceSizeXScaled = sprite.size.x / obj.transform.localScale.x;

            trueSizeX = sprite.size.x * obj.transform.localScale.x;
            trueSizeY = sprite.size.y * obj.transform.localScale.y;
        }
    }
}
