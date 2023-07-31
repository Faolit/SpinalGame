using UnityEngine;

namespace SpinalPlay
{
    public class RayRange
    {
        public Vector2 start, end;
        public RayRange(float xS, float yS, float xE, float yE)
        {
            start = new Vector2(xS, yS);
            end = new Vector2(xE, yE);
        }
    }
}
