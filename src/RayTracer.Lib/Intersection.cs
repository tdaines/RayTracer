using System;

namespace RayTracer.Lib
{
    public class Intersection : IComparable<Intersection>
    {
        public float Time { get; }
        public Shape Object { get; }

        public Intersection(float time, Shape obj)
        {
            Time = time;
            Object = obj;
        }

        public int CompareTo(Intersection other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}