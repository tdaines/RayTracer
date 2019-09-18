using System.IO;
using RayTracer.Lib;

namespace RayTracer.Exercises.SphereSilhouette
{
    public class Silhouette
    {
        public static void Run()
        {
            var rayOrigin = new Point(0, 0, -5);

            const float wallZ = 10;
            const float wallSize = 7.0f;
            const float halfWall = wallSize / 2.0f;
            const int canvasPixels = 300;
            const float pixelSize = wallSize / canvasPixels;
            
            var canvas = new Canvas(canvasPixels, canvasPixels, Color.Black);
            var sphere = new Lib.Sphere();

            for (int y = 0; y < canvas.Width; y++)
            {
                float worldY = halfWall - (pixelSize * y);

                for (int x = 0; x < canvas.Height; x++)
                {
                    float worldX = -halfWall + (pixelSize * x);
                    
                    // Point on wall that the ray will target
                    var position = new Point(worldX, worldY, wallZ);
                    
                    // Unit Vector pointing to position
                    var direction = Vector.Normalize(position - rayOrigin);
                    
                    // Ray pointing at position on wall, from origin
                    var ray = new Ray(rayOrigin, direction);

                    var intersections = sphere.Intersect(ray);
                    var intersection = intersections.Hit();

                    if (intersection != null)
                    {
                        canvas[x, y] = Color.Red;
                    }
                }
            }
            
            File.WriteAllLines("silhouette.ppm", canvas.GetPortablePixmap());
        }
    }
}