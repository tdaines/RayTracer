using RayTracer.Lib;
using RayTracer.Lib.Patterns;
using RayTracer.Lib.Shapes;
using Xunit;

namespace RayTracer.Test
{
    public class YamlParserTests
    {
        [Fact]
        public void AddCamera()
        {
            const string yaml = @"
- add: camera
  width: 100
  height: 100
  field-of-view: 0.785
  from: [ -6, 6, -10 ]
  to: [ 6, 0, 6 ]
  up: [ -0.45, 1, 0 ]";
            
            var parser = new YamlParser();
            var (_, camera) = parser.LoadYaml(yaml);
            
            var transform = ViewTransform.Create(
                new Point(-6, 6, -10),
                new Point(6, 0, 6),
                new Vector(-0.45f, 1, 0));
            
            Assert.Equal(100, camera.Width);
            Assert.Equal(100, camera.Height);
            Assert.Equal(0.785f, camera.FieldOfView);
            Assert.Equal(transform, camera.Transform);
        }

        [Fact]
        public void AddLight()
        {
            const string yaml = @"
- add: light
  at: [ 50, 100, -50 ]
  intensity: [ 1, 1, 1 ]";
            
            var parser = new YamlParser();
            var (world, _) = parser.LoadYaml(yaml);

            var light = world.Lights[0];
            Assert.Equal(new Point(50, 100, -50), light.Position);
            Assert.Equal(Color.White, light.Intensity);
        }

        [Fact]
        public void AddSphereWithSolidPatternMaterial()
        {
            const string yaml = @"
- add: sphere
  material:
    color: [ 1, 1, 1 ]
    ambient: 0
    diffuse: 0
    specular: 0.9
    shininess: 300
    reflective: 0.9
    transparency: 0.9
    refractive-index: 1.5";
            
            var parser = new YamlParser();
            var (world, _) = parser.LoadYaml(yaml);

            var sphere = (Sphere) world.Shapes[0];
            var material = sphere.Material;
            Assert.Equal(Matrix4x4.Identity(), sphere.Transform);
            Assert.Equal(Color.White, ((SolidPattern) material.Pattern).Color);
            Assert.Equal(0, material.Ambient);
            Assert.Equal(0, material.Diffuse);
            Assert.Equal(0.9f, material.Specular);
            Assert.Equal(300, material.Shininess);
            Assert.Equal(0.9f, material.Reflective);
            Assert.Equal(0.9f, material.Transparency);
            Assert.Equal(1.5f, material.RefractiveIndex);
        }
        
        [Fact]
        public void AddSphereWithSolidPatternMaterialAndTransform()
        {
            const string yaml = @"
- add: sphere
  transform:
    - [ scale, 0.5, 0.5, 0.5 ]
  material:
    color: [ 1, 1, 1 ]
    ambient: 0
    diffuse: 0
    specular: 0.9
    shininess: 300
    reflective: 0.9
    transparency: 0.9
    refractive-index: 1.5";
            
            var parser = new YamlParser();
            var (world, _) = parser.LoadYaml(yaml);

            var sphere = (Sphere) world.Shapes[0];
            var material = sphere.Material;
            Assert.Equal(Matrix4x4.Scaling(0.5f, 0.5f, 0.5f), sphere.Transform);
            Assert.Equal(Color.White, ((SolidPattern) material.Pattern).Color);
            Assert.Equal(0, material.Ambient);
            Assert.Equal(0, material.Diffuse);
            Assert.Equal(0.9f, material.Specular);
            Assert.Equal(300, material.Shininess);
            Assert.Equal(0.9f, material.Reflective);
            Assert.Equal(0.9f, material.Transparency);
            Assert.Equal(1.5f, material.RefractiveIndex);
        }
        
        [Fact]
        public void AddPlaneWithCheckeredMaterialAndTransform()
        {
            const string yaml = @"
- add: plane
  transform:
    - [ rotate-x, 1.5708 ]
    - [ translate, 0, 0, 10 ]
  material:
    pattern:
      type: checkers
      colors:
        - [ 0.15, 0.15, 0.15 ]
        - [ 0.85, 0.85, 0.85 ]
    ambient: 0.8
    diffuse: 0.2
    specular: 0";
            
            var parser = new YamlParser();
            var (world, _) = parser.LoadYaml(yaml);

            var plane = (Plane)world.Shapes[0];
            var material = plane.Material;
            var expectedTransform = Matrix4x4.Translation(0, 0, 10) * Matrix4x4.RotationX(1.5708f);
            
            Assert.Equal(expectedTransform, plane.Transform);
            Assert.True(material.Pattern is CheckeredPattern);
            
            var pattern = (CheckeredPattern) material.Pattern;
            Assert.Equal(new Color(0.15f, 0.15f, 0.15f), ((SolidPattern) pattern.Patterns[0]).Color);
            Assert.Equal(new Color(0.85f, 0.85f, 0.85f), ((SolidPattern) pattern.Patterns[1]).Color);
            
            Assert.Equal(0.8f, material.Ambient);
            Assert.Equal(0.2f, material.Diffuse);
            Assert.Equal(0, material.Specular);
            Assert.Equal(200, material.Shininess);
            Assert.Equal(0, material.Reflective);
            Assert.Equal(0, material.Transparency);
            Assert.Equal(1, material.RefractiveIndex);
        }

        [Fact]
        public void DefineMaterial()
        {
            const string yaml = @"
- define: floor-material
  value:
    color: [ 1, 0.9, 0.9 ]
    specular: 0
    ambient: 0.3";
            
            var parser = new YamlParser();
            parser.LoadYaml(yaml);
            
            Assert.Single(parser.Materials);

            var material = parser.Materials["FLOOR-MATERIAL"];
            var pattern = (SolidPattern) material.Pattern;
            
            Assert.Equal(new Color(1, 0.9f, 0.9f), pattern.Color);
            Assert.Equal(0.3f, material.Ambient);
            Assert.Equal(0.9f, material.Diffuse);
            Assert.Equal(0, material.Specular);
            Assert.Equal(200, material.Shininess);
            Assert.Equal(0, material.Reflective);
            Assert.Equal(0, material.Transparency);
            Assert.Equal(1, material.RefractiveIndex);
        }
        
        [Fact]
        public void DefineMaterialWithExtend()
        {
            const string yaml = @"
- define: white-material
  value:
    color: [ 1, 1, 1 ]
    diffuse: 0.7
    ambient: 0.1
    specular: 0.0
    reflective: 0.1

- define: blue-material
  extend: white-material
  value:
    color: [ 0.537, 0.831, 0.914 ]";
            
            var parser = new YamlParser();
            parser.LoadYaml(yaml);
            
            Assert.Equal(2, parser.Materials.Count);

            var whiteMaterial = parser.Materials["WHITE-MATERIAL"];
            var whitePattern = (SolidPattern) whiteMaterial.Pattern;
            
            Assert.Equal(Color.White, whitePattern.Color);
            Assert.Equal(0.1f, whiteMaterial.Ambient);
            Assert.Equal(0.7f, whiteMaterial.Diffuse);
            Assert.Equal(0, whiteMaterial.Specular);
            Assert.Equal(200, whiteMaterial.Shininess);
            Assert.Equal(0.1f, whiteMaterial.Reflective);
            Assert.Equal(0, whiteMaterial.Transparency);
            Assert.Equal(1, whiteMaterial.RefractiveIndex);
            
            var blueMaterial = parser.Materials["BLUE-MATERIAL"];
            var bluePattern = (SolidPattern) blueMaterial.Pattern;
            
            Assert.Equal(new Color(0.537f, 0.831f, 0.914f), bluePattern.Color);
            Assert.Equal(0.1f, blueMaterial.Ambient);
            Assert.Equal(0.7f, blueMaterial.Diffuse);
            Assert.Equal(0, blueMaterial.Specular);
            Assert.Equal(200, blueMaterial.Shininess);
            Assert.Equal(0.1f, blueMaterial.Reflective);
            Assert.Equal(0, blueMaterial.Transparency);
            Assert.Equal(1, blueMaterial.RefractiveIndex);
        }

        [Fact]
        public void AddSphereWithMaterialReference()
        {
            const string yaml = @"
- define: floor-material
  value:
    color: [ 1, 0.9, 0.9 ]
    specular: 0
    ambient: 0.3

- add: sphere
  material: floor-material
  transform:
    - [ scale, 10, 0.01, 10 ]";
            
            var parser = new YamlParser();
            var (world, _) = parser.LoadYaml(yaml);
            
            var floorMaterial = parser.Materials["FLOOR-MATERIAL"];
            var floorPattern = (SolidPattern) floorMaterial.Pattern;
            
            Assert.Equal(new Color(1, 0.9f, 0.9f), floorPattern.Color);
            Assert.Equal(0.3f, floorMaterial.Ambient);
            Assert.Equal(0.9f, floorMaterial.Diffuse);
            Assert.Equal(0, floorMaterial.Specular);
            Assert.Equal(200, floorMaterial.Shininess);
            Assert.Equal(0, floorMaterial.Reflective);
            Assert.Equal(0, floorMaterial.Transparency);
            Assert.Equal(1, floorMaterial.RefractiveIndex);
            
            var sphere = (Sphere) world.Shapes[0];
            
            Assert.Equal(Matrix4x4.Scaling(10, 0.01f, 10), sphere.Transform);
            Assert.Same(sphere.Material, floorMaterial);
        }
        
        [Fact]
        public void DefineMaterialWithStripesPattern()
        {
            const string yaml = @"
- define: wall-material
  value:
    pattern:
      type: stripes
      colors:
        - [0.45, 0.45, 0.45]
        - [0.55, 0.55, 0.55]
      transform:
        - [ scale, 0.25, 0.25, 0.25 ]
        - [ rotate-y, 1.5708 ]
    ambient: 0
    diffuse: 0.4
    specular: 0
    reflective: 0.3";
            
            var parser = new YamlParser();
            parser.LoadYaml(yaml);

            var material = parser.Materials["WALL-MATERIAL"];
            var pattern = (StripePattern) material.Pattern;
            var expectedTransform = Matrix4x4.RotationY(1.5708f)
                                  * Matrix4x4.Scaling(0.25f, 0.25f, 0.25f);
            
            Assert.Equal(expectedTransform, pattern.Transform);
            Assert.Equal(new Color(0.45f, 0.45f, 0.45f), ((SolidPattern) pattern.Patterns[0]).Color);
            Assert.Equal(new Color(0.55f, 0.55f, 0.55f), ((SolidPattern) pattern.Patterns[1]).Color);
            
            Assert.Equal(0, material.Ambient);
            Assert.Equal(0.4f, material.Diffuse);
            Assert.Equal(0, material.Specular);
            Assert.Equal(200, material.Shininess);
            Assert.Equal(0.3f, material.Reflective);
            Assert.Equal(0, material.Transparency);
            Assert.Equal(1, material.RefractiveIndex);
        }
    }
}