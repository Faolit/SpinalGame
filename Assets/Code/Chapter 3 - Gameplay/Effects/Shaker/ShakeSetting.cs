public class ShakeSetting
{
    public ShakeType shakeType;

    public float intense;
    public float duration;
    public float delay;

    public ShakeSetting(ShakeType shakeType, float intense, float duration, float delay)
    {
        this.shakeType = shakeType;
        this.intense = intense;
        this.duration = duration;
        this.delay = delay;
    }
}
