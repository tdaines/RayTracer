namespace RayTracer.Lib
{
    public class Intersection
    {
        public float Time { get; }
        public BaseObject Object { get; }

        public Intersection(float time, BaseObject obj)
        {
            Time = time;
            Object = obj;
        }
    }
}