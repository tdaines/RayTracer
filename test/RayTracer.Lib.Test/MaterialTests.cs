using RayTracer.Lib.Patterns;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class MaterialTests
    {
        [Fact]
        public void ConstructorDefaults()
        {
            var material = new Material();
            
            Assert.Equal(SolidPattern.White.Color, ((SolidPattern)material.Pattern).Color);
            Assert.Equal(0.1f, material.Ambient);
            Assert.Equal(0.9f, material.Diffuse);
            Assert.Equal(0.9f, material.Specular);
            Assert.Equal(200, material.Shininess);
            Assert.Equal(0, material.Reflective);
        }
        
        [Fact]
        public void ConstructorClamp()
        {
            var material = new Material(
                reflective: 1.5f,
                ambient: 1.5f,
                diffuse: 1.5f,
                specular: 1.5f);
            Assert.Equal(1, material.Reflective);
            Assert.Equal(1, material.Ambient);
            Assert.Equal(1, material.Diffuse);
            Assert.Equal(1, material.Specular);
            
            material = new Material(
                reflective: -0.5f,
                ambient: -0.5f,
                diffuse: -0.5f,
                specular: -0.5f);
            Assert.Equal(0, material.Reflective);
            Assert.Equal(0, material.Ambient);
            Assert.Equal(0, material.Diffuse);
            Assert.Equal(0, material.Specular);
        }
    }
}