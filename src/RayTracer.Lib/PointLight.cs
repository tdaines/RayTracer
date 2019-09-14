using System;

namespace RayTracer.Lib
{
    public class PointLight
    {
        public Color Intensity { get; }
        public Point Position { get; }

        public PointLight(Point position, Color intensity)
        {
            Position = position;
            Intensity = intensity;
        }

        public Color Lighting(Material material, Point point, Vector eye, Vector normal, bool inShadow)
        {
            // Combine surface color with light's color and intensity
            var effectiveColor = material.Color * Intensity;
            var ambientColor = effectiveColor * material.Ambient;
            
            if (inShadow)
            {
                return ambientColor;
            }
            
            // Find direction to light source
            var lightVector = Vector.Normalize(Position - point);
            var diffuseColor = Color.Black;
            var specularColor = Color.Black;

            // Find the cosine of the angle between the light vector
            // and the normal vector.  A negative value means the
            // light is on the other side of the surface
            var lightDotNormal = Vector.Dot(lightVector, normal);
            if (lightDotNormal >= 0)
            {
                diffuseColor = effectiveColor * material.Diffuse * lightDotNormal;
                
                // Find the cosine of the angle between the reflection vector
                // and the eye vector.  A negative value means the
                // light reflects away from the eye.
                var reflection = Vector.Reflect(-lightVector, normal);
                var reflectDotEye = Vector.Dot(reflection, eye);

                if (reflectDotEye > 0)
                {
                    var factor = MathF.Pow(reflectDotEye, material.Shininess);
                    specularColor = Intensity * material.Specular * factor;
                }
            }

            return ambientColor + diffuseColor + specularColor;
        }
    }
}