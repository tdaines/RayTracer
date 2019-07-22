using System;

namespace RayTracer.Lib
{
    public static class Extensions
    {
        public static bool ApproximatelyEquals(this float left, float right, float epsilon = 0.0001f)
        {
            // If they are equal anyway, just return True.
            if (left.Equals(right))
            {
                return true;
            }

            // Handle NaN, Infinity.
            if (float.IsInfinity(left) | float.IsNaN(left))
            {
                return left.Equals(right);
            }
            
            if (float.IsInfinity(right) | float.IsNaN(right))
            {
                return left.Equals(right);
            }

            // Handle zero to avoid division by zero
            float divisor = MathF.Max(left, right);
            if (divisor.Equals(0))
            {
                divisor = MathF.Min(left, right);
            }

            return MathF.Abs(left - right) / divisor <= epsilon;
        }
    }
}