namespace RayTracer.Lib
{
    public class Intersections
    {
        private readonly Intersection[] intersections;

        public Intersections(params Intersection[] intersections)
        {
            this.intersections = intersections;
        }

        public Intersection this[int index] => intersections[index];

        public int Count => intersections.Length;

        public Intersection Hit()
        {
            if (intersections.Length == 0)
            {
                return null;
            }

            Intersection closest = null;
            var closestTime = float.MaxValue;
            
            for (int i = 0; i < intersections.Length; i++)
            {
                var intersection = intersections[i];
                if (intersection.Time >= 0 && intersection.Time < closestTime)
                {
                    closest = intersections[i];
                    closestTime = closest.Time;
                }
            }
            
            return closest;
        }
    }
}