using System;
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

            sphere = new Sphere(Matrix4x4.Translation(2, 3, 4));
            
            Assert.Equal(Matrix4x4.Translation(2, 3, 4), sphere.Transform);
        }

        [Fact]
        public void Normal()
        {
            var sphere = new Sphere();
            
            Assert.Equal(new Vector(1, 0, 0), sphere.Normal(new Point(1, 0, 0)));
            Assert.Equal(new Vector(0, 1, 0), sphere.Normal(new Point(0, 1, 0)));
            Assert.Equal(new Vector(0, 0, 1), sphere.Normal(new Point(0, 0, 1)));
            Assert.Equal(new Vector(MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3), sphere.Normal(new Point(MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3)));
            
            var normal = sphere.Normal(new Point(MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3, MathF.Sqrt(3) / 3));
            
            Assert.Equal(Vector.Normalize(normal), normal);
        }
        
        [Fact]
        public void NormalTranslatedSphere()
        {
            var transform = Matrix4x4.Translation(0, 1, 0);
            var sphere = new Sphere(transform);

            var normal = sphere.Normal(new Point(0, 1.70711f, -0.70711f));
            
            Assert.Equal(new Vector(0, 0.70711f, -0.70711f), normal);
        }
        
        [Fact]
        public void NormalTransformedSphere()
        {
            var transform = Matrix4x4.Scaling(1, 0.5f, 1) * Matrix4x4.RotationZ(MathF.PI / 5);
            var sphere = new Sphere(transform);

            var normal = sphere.Normal(new Point(0, MathF.Sqrt(2) / 2, -MathF.Sqrt(2) / 2));
            
            Assert.Equal(new Vector(0, 0.97014f, -0.24254f), normal);
        }
        
        [Fact]
        public void Intersect()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = sphere.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(4.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Shape);
            Assert.Equal(6.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Shape);
        }
        
        [Fact]
        public void IntersectTangent()
        {
            var ray = new Ray(new Point(0, 1, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = sphere.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(5.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Shape);
            Assert.Equal(5.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Shape);
        }
        
        [Fact]
        public void IntersectMiss()
        {
            var ray = new Ray(new Point(0, 2, -5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = sphere.Intersect(ray);
            Assert.Empty(intersections);
        }
        
        [Fact]
        public void IntersectFromInside()
        {
            var ray = new Ray(new Point(0, 0, 0), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = sphere.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(-1.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Shape);
            Assert.Equal(1.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Shape);
        }
        
        [Fact]
        public void IntersectFromBehind()
        {
            var ray = new Ray(new Point(0, 0, 5), new Vector(0, 0, 1));
            var sphere = new Sphere();

            var intersections = sphere.Intersect(ray);
            Assert.Equal(2, intersections.Count);
            Assert.Equal(-6.0f, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Shape);
            Assert.Equal(-4.0f, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Shape);
        }
        
        [Fact]
        public void IntersectScaled()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Scaling(2, 2, 2));

            var intersections = sphere.Intersect(ray);
            
            Assert.Equal(2, intersections.Count);
            Assert.Equal(3, intersections[0].Time);
            Assert.Equal(sphere, intersections[0].Shape);
            Assert.Equal(7, intersections[1].Time);
            Assert.Equal(sphere, intersections[1].Shape);
        }
        
        [Fact]
        public void IntersectTranslated()
        {
            var ray = new Ray(new Point(0, 0, -5), new Vector(0, 0, 1));
            var sphere = new Sphere(Matrix4x4.Translation(5, 0, 0));

            var intersections = sphere.Intersect(ray);
            Assert.Empty(intersections);
        }
    }
}