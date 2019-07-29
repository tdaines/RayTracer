using System;
using Microsoft.VisualBasic.CompilerServices;
using Xunit;

namespace RayTracer.Lib.Test
{
    public class RayTests
    {
        [Fact]
        public void Constructor()
        {
            var origin = new Point(1, 2, 3);
            var direction = new Vector(4, 5, 6);
            
            var ray = new Ray(origin, direction);
            
            Assert.Equal(origin, ray.Origin);
            Assert.Equal(direction, ray.Direction);
        }
        
        [Fact]
        public void Position()
        {
            var ray = new Ray(new Point(2, 3, 4), new Vector(1, 0, 0));
            
            Assert.Equal(new Point(2, 3, 4), ray.Position(0));
            Assert.Equal(new Point(3, 3, 4), ray.Position(1));
            Assert.Equal(new Point(1, 3, 4), ray.Position(-1));
            Assert.Equal(new Point(4.5f, 3, 4), ray.Position(2.5f));
        }

        [Fact]
        public void IntersectSphere()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = ray.Intersect(sphere);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(4.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Object);
            Assert.Equal(6.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Object);
        }
        
        [Fact]
        public void IntersectSphereTangent()
        {
            var ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = ray.Intersect(sphere);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(5.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Object);
            Assert.Equal(5.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Object);
        }
        
        [Fact]
        public void IntersectSphereMiss()
        {
            var ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = ray.Intersect(sphere);
            Assert.Equal(0, intersections.Count);
        }
        
        [Fact]
        public void IntersectSphereFromInside()
        {
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = ray.Intersect(sphere);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(-1.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Object);
            Assert.Equal(1.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Object);
        }
        
        [Fact]
        public void IntersectSphereFromBehind()
        {
            var ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = ray.Intersect(sphere);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(-6.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Object);
            Assert.Equal(-4.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Object);
        }

        [Fact]
        public void TranslateTransform()
        {
            var ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var transform = Matrix4x4.Translation(3, 4, 5);

            ray = Ray.Transform(ray, transform);
            
            Assert.Equal(new Point(4, 6, 8), ray.Origin);
            Assert.Equal(new Vector(0, 1, 0), ray.Direction);
        }
        
        [Fact]
        public void ScaleTransform()
        {
            var ray = new Ray(new Point(1, 2, 3), new Vector(0, 1, 0));
            var transform = Matrix4x4.Scaling(2, 3, 4);

            ray = Ray.Transform(ray, transform);
            
            Assert.Equal(new Point(2, 6, 12), ray.Origin);
            Assert.Equal(new Vector(0, 3, 0), ray.Direction);
        }

        [Fact]
        public void IntersectSphereScaled()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Scaling(2, 2, 2));

            var intersections = ray.Intersect(sphere);
            
            Assert.Equal(2, intersections.Count);
            Assert.Equal(3, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Object);
            Assert.Equal(7, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Object);
        }
        
        [Fact]
        public void IntersectSphereTranslated()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Translation(5, 0, 0));

            var intersections = ray.Intersect(sphere);
            Assert.Equal(0, intersections.Count);
        }
    }
}