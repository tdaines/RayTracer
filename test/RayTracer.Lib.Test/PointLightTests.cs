using System;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class PointLightTests
    {
        [Fact]
        public void Lighting()
        {
            var material = new Material();
            var position = new Point(0, 0, 0);
            
            // Eye between light and the surface with normal pointing at eye
            var eye = new Vector(0, 0, -1);
            var normal = new Vector(0, 0, -1);
            var light = new PointLight(new Point(0, 0, -10), Color.White);

            var color = light.Lighting(material, position, eye, normal);
            Assert.Equal(new Color(1.9f, 1.9f, 1.9f), color);
            
            // Eye 45 degrees off surface normal
            eye = new Vector(0, MathF.Sqrt(2) / 2, -MathF.Sqrt(2) / 2);
            normal = new Vector(0, 0, -1);
            light = new PointLight(new Point(0, 0, -10), Color.White);
            
            color = light.Lighting(material, position, eye, normal);
            Assert.Equal(new Color(1.0f, 1.0f, 1.0f), color);
            
            // Normal pointing at eye, light 45 degrees off normal
            eye = new Vector(0, 0, -1);
            normal = new Vector(0, 0, -1);
            light = new PointLight(new Point(0, 10, -10), Color.White);
            
            color = light.Lighting(material, position, eye, normal);
            Assert.Equal(new Color(0.7364f, 0.7364f, 0.7364f), color);
            
            // Light 45 degrees off normal, eye in line with light reflection
            eye = new Vector(0, -MathF.Sqrt(2) / 2, -MathF.Sqrt(2) / 2);
            normal = new Vector(0, 0, -1);
            light = new PointLight(new Point(0, 10, -10), Color.White);
            
            color = light.Lighting(material, position, eye, normal);
            Assert.Equal(new Color(1.6364f, 1.6364f, 1.6364f), color);
            
            // Light behind surface
            eye = new Vector(0, 0, -1);
            normal = new Vector(0, 0, -1);
            light = new PointLight(new Point(0, 0, 10), Color.White);
            
            color = light.Lighting(material, position, eye, normal);
            Assert.Equal(new Color(0.1f, 0.1f, 0.1f), color);
        }
    }
}