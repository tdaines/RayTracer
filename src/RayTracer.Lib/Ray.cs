namespace RayTracer.Lib
{
    public struct Ray
    {
        public Point Origin { get; }
        public Vector Direction { get; }

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