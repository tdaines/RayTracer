using Xunit;

namespace RayTracer.Lib.Test
{
    public class SphereTests
    {
        [Fact]
        public void DefaultTransform()
        {
            var sphere = new Sphere();
            
            Assert.Equal(Matrix4x4.Identity(), sphere.Transform);

            sphere.Transform = Matrix4x4.Translation(2, 3, 4);
            
            Assert.Equal(Matrix4x4.Translation(2, 3, 4), sphere.Transform);
        }
    }
}