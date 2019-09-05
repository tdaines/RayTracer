using System;

namespace RayTracer.Lib
{
    public class Intersection : IComparable<Intersection>
    {
        public float Time { get; }
        public BaseObject Object { get; }

        public Intersection(float time, BaseObject obj)
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