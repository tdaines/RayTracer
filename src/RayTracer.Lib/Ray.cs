namespace RayTracer.Lib
{
    public class Ray
    {
        public Point Origin { get; }
        public Vector Direction { get; private set; }

        public Ray(Point origin, Vector direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Point Position(float time)
        {
            return Origin + (Direction * time);
        }
    }
}