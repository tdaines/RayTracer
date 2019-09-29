using System;
using RayTracer.Lib.Patterns;

namespace RayTracer.Lib
{
    public class Material
    {
        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public int Shininess { get; set; }
        public float Reflective { get; set; }
        public float Transparency { get; set; }
        public float RefractiveIndex { get; set; }
        public Pattern Pattern { get; set; }

        public Material(
            float ambient = 0.1f,
            float diffuse = 0.9f,
            float specular = 0.9f,
            int shininess = 200,
            float reflective = 0,
            float transparency = 0,
            float refractiveIndex = 1)
            : this(SolidPattern.White, ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex)
        {
        }
        
        public Material(
            Color color,
            float ambient = 0.1f,
            float diffuse = 0.9f,
            float specular = 0.9f,
            int shininess = 200,
            float reflective = 0,
            float transparency = 0,
            float refractiveIndex = 1)
            : this(new SolidPattern(color), ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex)
        {
        }
        
        public Material(
            Pattern pattern,
            float ambient = 0.1f,
            float diffuse = 0.9f,
            float specular = 0.9f,
            int shininess = 200,
            float reflective = 0,
            float transparency = 0,
            float refractiveIndex = 1)
        {
            Pattern = pattern;
            Ambient = Clamp(ambient, 0, 1);
            Diffuse = Clamp(diffuse, 0, 1);
            Specular = Clamp(specular, 0, 1);
            Shininess = shininess;
            Reflective = Clamp(reflective, 0, 1);
            Transparency = Clamp(transparency, 0, 1);
            RefractiveIndex = Clamp(refractiveIndex, 0, float.MaxValue);
        }
        
        private float Clamp(float value, float min, float max)
        {
            return MathF.Min(max, MathF.Max(min, value));
        }
        
        public static Material Mirror(
            float ambient = 0,
            float diffuse = 0,
            float specular = 0.9f,
            int shininess = 2000,
            float reflective = 1,
            float transparency = 0,
            float refractiveIndex = 1)
        {
            return new Material(SolidPattern.Black, ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex);
        }

        public static Material Glass(
            float ambient = 0,
            float diffuse = 0,
            float specular = 0.9f,
            int shininess = 200,
            float reflective = 1,
            float transparency = 1,
            float refractiveIndex = 1.5f)
        {
            return new Material(ambient, diffuse, specular, shininess, reflective, transparency, refractiveIndex);
        }
    }
}