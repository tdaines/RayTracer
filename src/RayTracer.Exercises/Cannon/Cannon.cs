using System.IO;
using RayTracer.Lib;

namespace RayTracer.Exercises.Cannon
{
    public class Cannon
    {
        public void Run()
        {
            var canvas = new Canvas(900, 550);
            
            var gravity = new Vector(0, -0.1f, 0);
            var wind = new Vector(-0.01f, 0, 0);
            var environment = new Environment(gravity, wind);
            
            var position = new Point(0, 1, 0);
            var velocity = Vector.Normalize(new Vector(1, 1.8f, 0)) * 11.25f;
            var projectile = new Projectile(position, velocity);

            while (projectile.Position.Y >= 0)
            {
                int col = (int)projectile.Position.X;
                int row = canvas.Height - (int) projectile.Position.Y;

                canvas[col, row] = Color.Red;

                projectile = Tick(environment, projectile);
            }
            
            File.WriteAllText("cannon.ppm", canvas.GetPortablePixmap());
        }

        private Projectile Tick(Environment environment, Projectile projectile)
        {
            var position = projectile.Position + projectile.Velocity;
            var velocity = projectile.Velocity + environment.Gravity + environment.Wind;
            
            return new Projectile(position, velocity);
        }
    }

    public class Environment
    {
        public Vector Gravity { get; }
        public Vector Wind { get; }

        public Environment(Vector gravity, Vector wind)
        {
            Gravity = gravity;
            Wind = wind;
        }
    }

    public class Projectile
    {
        public Point Position { get; }
        public Vector Velocity { get; }

        public Projectile(Point position, Vector velocity)
        {
            Position = position;
            Velocity = velocity;
        }
    }
}