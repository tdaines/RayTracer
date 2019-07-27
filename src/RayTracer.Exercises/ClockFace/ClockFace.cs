using System;
using System.IO;
using RayTracer.Lib;

namespace RayTracer.Exercises.ClockFace
{
    public class ClockFace
    {
        public void Run()
        {
            var hours = new Point[12];
            var twelve = new Point(0, 1, 0);
            hours[0] = twelve;

            for (int i = 1; i < hours.Length; i++)
            {
                hours[i] = twelve * Matrix4x4.RotationZ(i * MathF.PI / 6);
            }
            
            var radius = 180;
            var canvas = new Canvas(400, 400);

            for (int i = 0; i < hours.Length; i++)
            {
                hours[i] = hours[i] * Matrix4x4.Scaling(radius, radius, 1);
                hours[i] = hours[i] * Matrix4x4.Translation(canvas.Height / 2.0f, canvas.Height / 2.0f, 0);
                canvas.SetPixel(hours[i], Color.White);
            }
            
            File.WriteAllLines("clockface.ppm", canvas.GetPortablePixmap());
        }
    }
}