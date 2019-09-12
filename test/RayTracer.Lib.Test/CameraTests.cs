using System;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class CameraTests
    {
        [Fact]
        public void Constructor()
        {
            var camera = new Camera(160, 120, MathF.PI / 2);
            
            Assert.Equal(160, camera.Width);
            Assert.Equal(120, camera.Height);
            Assert.Equal(MathF.PI / 2, camera.FieldOfView);
            Assert.Equal(Matrix4x4.Identity(), camera.Transform);
        }

        [Fact]
        public void PixelSize()
        {
            // horizontal canvas
            var camera = new Camera(200, 125, MathF.PI / 2);
            Assert.Equal(0.01f, camera.PixelSize);
            
            // vertical canvas
            camera = new Camera(125, 200, MathF.PI / 2);
            Assert.Equal(0.01f, camera.PixelSize);
        }

        [Fact]
        public void RayForPixelCenterCanvas()
        {
            var camera = new Camera(201, 101, MathF.PI / 2);
            
            var ray = camera.RayForPixel(100, 50);
            
            Assert.Equal(new Point(0, 0, 0), ray.Origin);
            Assert.Equal(new Vector(0, 0, -1), ray.Direction);
        }
        
        [Fact]
        public void RayForPixelCornerCanvas()
        {
            var camera = new Camera(201, 101, MathF.PI / 2);
            
            var ray = camera.RayForPixel(0, 0);
            
            Assert.Equal(new Point(0, 0, 0), ray.Origin);
            Assert.Equal(new Vector(0.66519f, 0.33259f, -0.66851f), ray.Direction);
        }
        
        [Fact]
        public void RayForPixelTransformedCamera()
        {
            var transform = Matrix4x4.RotationY(MathF.PI / 4) * Matrix4x4.Translation(0, -2, 5);
            var camera = new Camera(201, 101, MathF.PI / 2, transform);
            
            var ray = camera.RayForPixel(100, 50);
            
            Assert.Equal(new Point(0, 2, -5), ray.Origin);
            Assert.Equal(new Vector(MathF.Sqrt(2) / 2, 0, -MathF.Sqrt(2) / 2), ray.Direction);
        }
    }
}