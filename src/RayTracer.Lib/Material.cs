namespace RayTracer.Lib
{
    public class Material
    {
        public Color Color { get; }
        public float Ambient { get; }
        public float Diffuse { get; }
        public float Specular { get; }
        public float Shininess { get; }

        public Material(float ambient = 0.1f, float diffuse = 0.9f, float specular = 0.9f, float shininess = 200)
            : this(Color.White, ambient, diffuse, specular, shininess)
        {
        }
        
        public Material(Color color, float ambient = 0.1f, float diffuse = 0.9f, float specular = 0.9f, float shininess = 200)
        {
            Color = color;
            Ambient = ambient;
            Diffuse = diffuse;
            Specular = specular;
            Shininess = shininess;
        }
    }
}