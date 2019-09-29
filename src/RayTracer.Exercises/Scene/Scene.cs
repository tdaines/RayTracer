using System;
using System.IO;
using RayTracer.Lib;
using RayTracer.Lib.Patterns;

namespace RayTracer.Exercises.Scene
{
    public class Scene
    {
        public static void Run()
        {
            JustFloor();
            return;
            
            var floorMaterial = new Material(new Color(1, 0.9f, 0.9f), specular: 0, shininess: 200, reflective: 0.1f);
            floorMaterial.Pattern = new CheckeredPattern(Matrix4x4.Scaling(1, 1, 1),
                new GradientPattern(Matrix4x4.RotationZ(MathF.PI / 2) * Matrix4x4.Scaling(0.5f, 0.5f, 0.5f)), 
                new StripePattern(Matrix4x4.RotationX(MathF.PI / 2) * Matrix4x4.Scaling(0.5f, 0.5f, 0.5f), SolidPattern.Blue, SolidPattern.Red));

            floorMaterial.Pattern = new BlendedPattern(
                new StripePattern(SolidPattern.Green, SolidPattern.White),
                new StripePattern(Matrix4x4.RotationY(MathF.PI / 2.0f), SolidPattern.Green, SolidPattern.White));

            var floor = new Plane(floorMaterial);

            var leftWallTransform = Matrix4x4.Translation(0, 0, 3) *
                                    Matrix4x4.RotationX(MathF.PI / 2);
            var wall = new Plane(leftWallTransform, new Material() { Pattern = new RingPattern(new RadialGradientPattern(), SolidPattern.Green)}); 
            
//            var leftWallTransform = Matrix4x4.Translation(0, 0, 5) *
//                                    Matrix4x4.RotationY(-MathF.PI / 4) *
//                                    Matrix4x4.RotationX(MathF.PI / 2) *
//                                    Matrix4x4.Scaling(10, 0.01f, 10);
            var leftWall = new Lib.Sphere(leftWallTransform, floorMaterial);
            
            var rightWallTransform = Matrix4x4.Translation(0, 0, 5) *
                                     Matrix4x4.RotationY(MathF.PI / 4) *
                                     Matrix4x4.RotationX(MathF.PI / 2) *
                                     Matrix4x4.Scaling(10, 0.01f, 10);
            var rightWall = new Lib.Sphere(rightWallTransform, floorMaterial);
            
            var middleTransform = Matrix4x4.Translation(-0.5f, 1, 0.5f);
            var middleMaterial = new Material(Color.Black, diffuse: 0.7f, specular: 0.3f, reflective: 1);
//            middleMaterial.Pattern = new BlendedPattern(Matrix4x4.Scaling(0.25f, 0.25f, 0.25f),
//                new StripePattern(SolidPattern.Green, SolidPattern.White),
//                new StripePattern(Matrix4x4.RotationY(MathF.PI / 4.0f), SolidPattern.Green, SolidPattern.White));
            var middle = new Lib.Sphere(middleTransform, middleMaterial);
            
            var rightTransform = Matrix4x4.Translation(1.5f, 0.5f, -0.5f) *
                                 Matrix4x4.Scaling(0.5f, 0.5f, 0.5f);
            var rightMaterial = new Material(new Color(0.5f, 1, 0.1f), diffuse: 0.7f, specular: 1.3f, shininess: 200);
            rightMaterial.Pattern = new GradientPattern(Matrix4x4.Translation(5, 0, 0) *
                                                        Matrix4x4.Scaling(2, 2, 2), Color.Red, Color.Blue);
            var right = new Lib.Sphere(rightTransform, rightMaterial);
            
            var leftTransform = Matrix4x4.Translation(-1.5f, 0.33f, -0.75f) *
                                Matrix4x4.Scaling(0.33f, 0.33f, 0.33f);
            var leftMaterial = new Material(new Color(1, 0.8f, 0.1f), diffuse: 0.7f, specular: 0.3f);
            leftMaterial.Pattern = new StripePattern(Matrix4x4.Scaling(0.75f, 0.75f, 0.75f) * Matrix4x4.RotationY(MathF.PI / 4));
            var left = new Lib.Sphere(leftTransform, leftMaterial);
            
            var light = new PointLight(new Point(-10, 10, -10), new Color(0.5f, 0.5f, 0.5f));
            var light2 = new PointLight(new Point(10, 10, -10), new Color(0.2f, 0.2f, 0.2f));
            var light3 = new PointLight(new Point(0, 10, -10), new Color(0.2f, 0.2f, 0.2f));
            
            var from = new Point(0, 1.5f, -5);
            var to = new Point(0, 1, 0);
            var up = new Vector(0, 1, 0);
            var cameraTransform = ViewTransform.Create(from, to, up);
            var camera = new Camera(400, 200, MathF.PI / 3, cameraTransform);
            
            var world = new World();
            world.Lights.Add(light);
            world.Lights.Add(light2);
            world.Lights.Add(light3);
            world.Shapes.Add(floor);
            world.Shapes.Add(wall);
//            world.Shapes.Add(leftWall);
//            world.Shapes.Add(rightWall);
            world.Shapes.Add(middle);
            world.Shapes.Add(right);
            world.Shapes.Add(left);
            
            var canvas = world.Render(camera);
            
            File.WriteAllLines("scene.ppm", canvas.GetPortablePixmap());
        }

        private static void JustFloor()
        {
            var floorMaterial = new Material(new Color(1, 0.9f, 0.9f), specular: 0);

            floorMaterial.Pattern = new BlendedPattern(
                new StripePattern(SolidPattern.Green, SolidPattern.White),
                new StripePattern(Matrix4x4.RotationY(MathF.PI / 2.0f), SolidPattern.Green, SolidPattern.White));

            floorMaterial.Pattern = new CheckeredPattern(
                new StripePattern(Matrix4x4.RotationY(MathF.PI / -4) * Matrix4x4.Scaling(0.25f, 0.25f, 0.25f), SolidPattern.Green, SolidPattern.Yellow), 
                new StripePattern(Matrix4x4.RotationY(MathF.PI / 4) * Matrix4x4.Scaling(0.5f, 0.5f, 0.5f),
                    new StripePattern(Matrix4x4.RotationY(MathF.PI / 4) * Matrix4x4.Scaling(0.15f, 0.15f, 0.15f)), SolidPattern.Red));
//            
//            floorMaterial.Pattern = new CheckeredPattern(
//                new GradientPattern(), 
//                new GradientPattern(Matrix4x4.RotationY(MathF.PI / 2.0f), Color.Blue, Color.Red));

//            floorMaterial.Pattern = new RingPattern(
//                new GradientPattern(),
//                new GradientPattern(Matrix4x4.RotationY(MathF.PI / 2), Color.Blue, Color.Red));
//            
//            floorMaterial.Pattern = new BlendedPattern(
//                new RingPattern(SolidPattern.Green, SolidPattern.White),
//                new StripePattern(SolidPattern.Green, SolidPattern.White));
            
//            floorMaterial.Pattern = new RingPattern();

            
//            floorMaterial.Pattern = new StripePattern(Matrix4x4.Scaling(0.25f, 0.25f, 0.25f) * Matrix4x4.RotationY(MathF.PI / 2));
//            floorMaterial.Pattern = new StripePattern(Matrix4x4.RotationY(MathF.PI / 2.0f), SolidPattern.Green, SolidPattern.White);
            
            var floor = new Plane(floorMaterial);

            var sphere = new Lib.Sphere(Matrix4x4.Translation(0, 1, 0), Material.Mirror());
            
            var smallSphere = new Lib.Sphere(
                Matrix4x4.Translation(2, 1, -1) * Matrix4x4.Scaling(0.5f, 0.5f, 0.5f),
                new Material(Color.Blue));
            
            var light = new PointLight(new Point(-10, 10, -10), new Color(0.5f, 0.5f, 0.5f));
//            var light2 = new PointLight(new Point(10, 10, -10), new Color(0.2f, 0.2f, 0.2f));
//            var light3 = new PointLight(new Point(0, 10, -10), new Color(0.2f, 0.2f, 0.2f));
            
            var from = new Point(0, 1.5f, -5);
            var to = new Point(0, 1, 0);
            var up = new Vector(0, 1, 0);
            var cameraTransform = ViewTransform.Create(from, to, up);
            var camera = new Camera(400, 200, MathF.PI / 3, cameraTransform);
            
            var world = new World();
            world.Lights.Add(light);
            world.Shapes.Add(sphere);
            world.Shapes.Add(smallSphere);
//            world.Lights.Add(light2);
//            world.Lights.Add(light3);
            world.Shapes.Add(floor);
            
            var canvas = world.Render(camera);
            
            File.WriteAllLines("floor.ppm", canvas.GetPortablePixmap());
        }
    }
}