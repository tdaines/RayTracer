using Xunit;

namespace RayTracer.Lib.Test
{
    public class MaterialTests
    {
        [Fact]
        public void ConstructorDefaults()
        {
            var material = new Material();
            
            Assert.Equal(Color.White, material.Color);
            Assert.Equal(0.1f, material.Ambient);
            Assert.Equal(0.9f, material.Diffuse);
            Assert.Equal(0.9f, material.Specular);
            Assert.Equal(200, material.Shininess);
        }
    }
}