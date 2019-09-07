using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RayTracer.Lib
{
    public class Intersections : IEnumerable<Intersection>
    {
        private readonly Intersection[] intersections;

        public Intersections(params Intersection[] intersections)
        {
            this.intersections = intersections;
            Array.Sort(this.intersections);
        }

        public Intersections(IEnumerable<Intersection> intersections)
        {
            this.intersections = intersections.ToArray();
            Array.Sort(this.intersections);
        }

        public Intersection this[int index] => intersections[index];

        public int Count => intersections.Length;

        public Intersection Hit()
        {
            if (intersections.Length == 0)
            {
                return null;
            }

            for (int i = 0; i < intersections.Length; i++)
            {
                var intersection = intersections[i];
                if (intersection.Time >= 0)
                {
                    return intersection;
                }
            }

            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Intersection> GetEnumerator()
        {
            for (int i = 0; i < intersections.Length; i++)
            {
                yield return intersections[i];
            }
        }
    }
}