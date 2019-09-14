namespace RayTracer.Lib
{
    public class IntersectionInfo
    {
        public Intersection Intersection { get; }
        public Point Point { get; }
        public Point OverPoint { get; }
        public Vector EyeVector { get; }
        public Vector Normal { get; }
        public bool Inside { get; }

        public IntersectionInfo(Intersection intersection, Ray ray)
        {
            Intersection = intersection;
            Point = ray.Position(intersection.Time);
            EyeVector = -ray.Direction;
            Normal = intersection.Object.Normal(Point);
            Inside = false;

            if (Vector.Dot(Normal, EyeVector) < 0)
            {
                Inside = true;
                Normal = -Normal;
            }

            OverPoint = Point + Normal * 0.01f;
        }
    }
}