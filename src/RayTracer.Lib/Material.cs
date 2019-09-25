using RayTracer.Lib.Patterns;

namespace RayTracer.Lib
{
    public class Material
    {
        public float Ambient { get; set; }
        public float Diffuse { get; set; }
        public float Specular { get; set; }
        public float Shininess { get; set; }
        public Pattern Pattern { get; set; }

        public Material(float ambient = 0.1f, float diffuse = 0.9f, float specular = 0.9f, float shininess = 200)
            : this(SolidPattern.White, ambient, diffuse, specular, shininess)
        {
        }
        
        public Material(Pattern pattern, float ambient = 0.1f, float diffuse = 0.9f, float specular = 0.9f, float shininess = 200)
        {
            Pattern = pattern;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }
        
        public Material(Color color, float ambient = 0.1f, float diffuse = 0.9f, float specular = 0.9f, float shininess = 200)
        {
            Pattern = new SolidPattern(color);
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }
    }
}