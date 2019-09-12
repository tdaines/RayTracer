namespace RayTracer.Lib
{
    public static class ViewTransform
    {
        public static Matrix4x4 Create(Point from, Point to, Vector up)
        {
            var forward = Vector.Normalize(to - from);
            var upNormalized = Vector.Normalize(up);
            var left = Vector.Cross(forward, upNormalized);
            var trueUp = Vector.Cross(left, forward);
            
            var orientation = new Matrix4x4(
                 left.X,     left.Y,     left.Z,    0,
                 trueUp.X,   trueUp.Y,   trueUp.Z,  0,
                -forward.X, -forward.Y, -forward.Z, 0,
                 0,           0,          0,        1);

            return orientation * Matrix4x4.Translation(-from.X, -from.Y, -from.Z);
        }
    }
}