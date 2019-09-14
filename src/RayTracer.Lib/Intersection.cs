using System;

namespace RayTracer.Lib
{
    public class Intersection : IComparable<Intersection>
    {
        public float Time { get; }
        public Shape Object { get; }

        public Intersection(float time, Shape shape)
        {
            Time = time;
            Object = shape;
        }

        public int CompareTo(Intersection other)
        {
            return Time.CompareTo(other.Time);
        }
    }
}