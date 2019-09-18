using Xunit;

namespace RayTracer.Lib.Test
{
    public class ShapeTests
    {
        [Fact]
        public void TranslateTransform()
        {
            var ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            Shape shape = new Sphere(Matrix4x4.Inverse(Matrix4x4.Translation(3, 4, 5)));

            ray = shape.TransformRay(ray);
            
            Assert.Equal(new Point(4, 6, 8), ray.Origin);
            Assert.Equal(new Vector(0, 1, 0), ray.Direction);
        }
        
        [Fact]
        public void ScaleTransform()
        {
            var ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            Shape shape = new Sphere(Matrix4x4.Inverse(Matrix4x4.Scaling(2, 3, 4)));

            ray = shape.TransformRay(ray);
            
            Assert.Equal(new Point(2, 6, 12), ray.Origin);
            Assert.Equal(new Vector(0, 3, 0), ray.Direction);
        }
    }
}