using System;

namespace RayTracer.Lib
{
    public class Camera
    {
        public int Width { get; }
        public int Height { get; }
        public float FieldOfView { get; }
        public Matrix4x4 Transform { get; }
        public float PixelSize { get; }

        private float halfWidth;
        private float halfHeight;
        private readonly Matrix4x4 inverseTransform;
        
        public Camera(int width, int height, float fieldOfView) : this(width, height, fieldOfView, Matrix4x4.Identity())
        {
        }

        public Camera(int width, int height, float fieldOfView, Matrix4x4 transform)
        {
            Width = width;
            Height = height;
            FieldOfView = fieldOfView;
            Transform = transform;
            inverseTransform = Matrix4x4.Inverse(Transform);
            PixelSize = CalculatePixelSize();
        }

        private float CalculatePixelSize()
        {
            var halfView = MathF.Tan(FieldOfView / 2);
            var aspectRatio = Width / (float)Height;

            if (aspectRatio >= 1)
            {
                halfWidth = halfView;
                halfHeight = halfView / aspectRatio;
            }
            else
            {
                halfWidth = halfView * aspectRatio;
                halfHeight = halfView;
            }

            return (halfWidth * 2) / Width;
        }

        public Ray RayForPixel(int x, int y, float xOffset = 0.5f, float yOffset = 0.5f)
        {
            // offset from edge of the canvas to point within pixel
            xOffset = (x + xOffset) * PixelSize;
            yOffset = (y + yOffset) * PixelSize;

            // the untransformed coordinates of the pixel in world space.
            // (remember that the camera looks toward -z, so +x is the the left)
            var worldX = halfWidth - xOffset;
            var worldY = halfHeight - yOffset;

            // using the camera matrix, transform the canvas point and the origin,
            // and then compute the ray's direction vector.
            // (remember that the canvas is at z = -1)
            var pixel = inverseTransform * new Point(worldX, worldY, -1);
            var origin = inverseTransform * Point.Zero;
            var direction = Vector.Normalize(pixel - origin);
            
            return new Ray(origin, direction);
        }
    }
}