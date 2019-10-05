using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RayTracer.Lib;
using RayTracer.Lib.Patterns;
using RayTracer.Lib.Shapes;
using YamlDotNet.RepresentationModel;

namespace RayTracer
{
    public class YamlParser
    {
        public Dictionary<string, Material> Materials { get; private set; }
        public Dictionary<string, Matrix4x4> Transforms { get; private set; }
        public Dictionary<string, Matrix4x4> Objects { get; private set; }

        public (World, Camera) LoadYamlFile(string file)
        {
            using var reader = new StreamReader(file);
            string yaml = reader.ReadToEnd();
            return LoadYaml(yaml);
        }
        
        public (World, Camera) LoadYaml(string yaml)
        {
            var world = new World();
            Camera camera = null;
            Materials = new Dictionary<string, Material>();
            Transforms = new Dictionary<string, Matrix4x4>();
            Objects = new Dictionary<string, Matrix4x4>();
            
            using var reader = new StringReader(yaml);
            var yamlStream = new YamlStream();
            yamlStream.Load(reader);
            
            var mapping = (YamlSequenceNode)yamlStream.Documents[0].RootNode;

            foreach (var node in mapping)
            {
                var nodes = node.AllNodes.ToArray();

                string command = ((YamlScalarNode) nodes[1]).Value.ToUpperInvariant();
                string type = ((YamlScalarNode) nodes[2]).Value.ToUpperInvariant();

                switch (command)
                {
                    case "ADD":
                        switch (type)
                        {
                            case "CAMERA":
                                camera = ParseCamera(node);
                                break;
                            case "LIGHT":
                                world.Lights.Add(ParseLight(node));
                                break;
                            case "PLANE":
                            case "SPHERE":
                            case "CUBE":
                                world.Shapes.Add(ParseShape(node));
                                break;
                            default:
                                throw new Exception($"Add {type} not supported (line {node.Start.Line})");
                        }
                        break;
                    case "DEFINE":
                        var name = type;
                        type = type.Substring(type.LastIndexOf('-') + 1);

                        switch (type)
                        {
                            case "MATERIAL":
                                Materials.Add(name, ParseMaterialDefine(node));
                                break;
                            case "TRANSFORM":
                                Transforms.Add(name, ParseTransform(node.AllNodes.ToArray()[4]));
                                break;
                            case "OBJECT":
                                Objects.Add(name, ParseTransform(node.AllNodes.ToArray()[4]));
                                break;
                            default:
                                throw new Exception($"Define {name} not supported (line {node.Start.Line})");
                        }
                        break;
                    default:
                        throw new Exception($"Command {command} not supported (line {node.Start.Line})");
                }
            }
            
            return (world, camera);
        }

        private Shape ParseShape(YamlNode node)
        {
            string type = null;
            var transform = Matrix4x4.Identity();
            var material = new Material();
            
            string[] supportedShapes = { "SPHERE", "PLANE", "CUBE" };
            
            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();

                switch (key)
                {
                    case "ADD":
                        type = ((YamlScalarNode) child.Value).Value.ToUpperInvariant();
                        if (!supportedShapes.Contains(type))
                        {
                            throw new Exception($"Shape type {type} not supported (line {child.Key.Start.Line})");
                        }
                        break;
                    case "TRANSFORM":
                        transform = ParseTransform(child.Value);
                        break;
                    case "MATERIAL":
                        material = ParseMaterialDefine(child.Value);
                        break;
                    default:
                        throw new Exception($"Shape attribute {key} not supported (line {child.Key.Start.Line})");
                }
            }

            switch (type)
            {
                case "PLANE":
                    return new Plane(transform, material);
                case "SPHERE":
                    return new Sphere(transform, material);
                case "CUBE":
                    return new Cube(transform, material);
                default:
                    throw new Exception($"Shape type {type} not supported (line {node.Start.Line})");
            }
        }

        private PointLight ParseLight(YamlNode node)
        {
            var intensity = Color.Black;
            var at = Point.Zero;

            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();

                switch (key)
                {
                    case "INTENSITY":
                        intensity = ParseColor(child.Value);
                        break;
                    case "AT":
                        at = ParsePoint(child.Value);
                        break;
                    case "ADD": break;
                    default:
                        throw new Exception($"Light attribute {key} not supported (line {child.Key.Start.Line})");
                }
            }
            
            return new PointLight(at, intensity);
        }

        private Camera ParseCamera(YamlNode node)
        {
            var width = 0;
            var height = 0;
            float fieldOfView = 0;

            var from = Point.Zero;
            var to = Point.Zero;
            var up = Vector.UnitY;
            
            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();

                switch (key)
                {
                    case "WIDTH":
                        width = ParseInt(child.Value);
                        break;
                    case "HEIGHT":
                        height = ParseInt(child.Value);
                        break;
                    case "FIELD-OF-VIEW":
                        fieldOfView = ParseFloat(child.Value);
                        break;
                    case "FROM":
                        from = ParsePoint(child.Value);
                        break;
                    case "TO":
                        to = ParsePoint(child.Value);
                        break;
                    case "UP":
                        up = ParseVector(child.Value);
                        break;
                    case "ADD": break;
                    default:
                        throw new Exception($"Camera attribute {key} not supported (line {child.Key.Start.Line})");
                }
            }
            
            var cameraTransform = ViewTransform.Create(from, to, up);
            return new Camera(width, height, fieldOfView, cameraTransform);
        }

        private static int ParseInt(YamlNode node)
        {
            return int.Parse(((YamlScalarNode)node).Value);
        }
        
        private static float ParseFloat(YamlNode node)
        {
            return float.Parse(((YamlScalarNode)node).Value);
        }

        private static Point ParsePoint(YamlNode node)
        {
            var sequence = (YamlSequenceNode) node;
            return new Point(ParseFloat(sequence[0]), ParseFloat(sequence[1]), ParseFloat(sequence[2]));
        }

        private static Vector ParseVector(YamlNode node)
        {
            var sequence = (YamlSequenceNode) node;
            return new Vector(ParseFloat(sequence[0]), ParseFloat(sequence[1]), ParseFloat(sequence[2]));
        }
        
        private static Color ParseColor(YamlNode node)
        {
            var sequence = (YamlSequenceNode) node;
            return new Color(ParseFloat(sequence[0]), ParseFloat(sequence[1]), ParseFloat(sequence[2]));
        }

        private Matrix4x4 ParseTransform(YamlNode node)
        {
            var transform = Matrix4x4.Identity();

            foreach (var child in (YamlSequenceNode)node)
            {
                string type;
                
                if (child.NodeType == YamlNodeType.Scalar)
                {
                    string key = ((YamlScalarNode) child).Value.ToUpperInvariant();
                    type = key.Substring(key.LastIndexOf('-') + 1);

                    switch (type)
                    {
                        case "OBJECT":
                            transform = Objects[key] * transform;
                            break;
                        case "TRANSFORM":
                            transform = Transforms[key] * transform;
                            break;
                        default:
                            throw new Exception($"Defined {key} not supported (line {child.Start.Line})");
                    }

                    continue;
                }
                
                type = ((YamlScalarNode) child[0]).Value.ToUpperInvariant();
                
                switch (type)
                {
                    case "TRANSLATE":
                        transform = Matrix4x4.Translation(ParseFloat(child[1]), ParseFloat(child[2]), ParseFloat(child[3])) * transform;
                        break;
                    case "SCALE":
                        transform = Matrix4x4.Scaling(ParseFloat(child[1]), ParseFloat(child[2]), ParseFloat(child[3])) * transform;
                        break;
                    case "ROTATE-X":
                        transform = Matrix4x4.RotationX(ParseFloat(child[1])) * transform;
                        break;
                    case "ROTATE-Y":
                        transform = Matrix4x4.RotationY(ParseFloat(child[1])) * transform;
                        break;
                    case "ROTATE-Z":
                        transform = Matrix4x4.RotationZ(ParseFloat(child[1])) * transform;
                        break;
                    default:
                        throw new Exception($"Transform {type} not supported (line {child.Start.Line})");
                }
            }

            return transform;
        }

        private Material Copy(Material from)
        {
            return new Material
            {
                Ambient = from.Ambient,
                Diffuse = from.Diffuse,
                Pattern = from.Pattern,
                Reflective = from.Reflective,
                RefractiveIndex = from.RefractiveIndex,
                Shininess = from.Shininess,
                Specular = from.Specular,
                Transparency = from.Transparency
            };
        }
        
        private Material ParseMaterialDefine(YamlNode node)
        {
            if (node.NodeType == YamlNodeType.Scalar)
            {
                string key = ((YamlScalarNode) node).Value.ToUpperInvariant();
                return Materials[key];
            }

            if (node.NodeType == YamlNodeType.Mapping)
            {
                Material materialToExtend = null;
                
                foreach (var child in (YamlMappingNode) node)
                {
                    var keyNode = (YamlScalarNode) child.Key;
                    var key = keyNode.Value.ToUpperInvariant();
                    switch (key)
                    {
                        case "DEFINE":
                            break;
                        case "EXTEND":
                            var valueNode = (YamlScalarNode) child.Value;
                            var value = valueNode.Value.ToUpperInvariant();
                            materialToExtend = Materials[value];
                            break;
                        case "VALUE":
                            return ParseMaterial(child.Value, materialToExtend);
                    }
                }
            }
            
            return ParseMaterial(node);
        }

        private Material ParseMaterial(YamlNode node, Material material = null)
        {
            if (material == null)
            {
                material = new Material();
            }
            else
            {
                material = Copy(material);
            }

            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();
                switch (key)
                {
                    case "AMBIENT":
                        material.Ambient = ParseFloat(child.Value);
                        break;
                    case "DIFFUSE":
                        material.Diffuse = ParseFloat(child.Value);
                        break;
                    case "SPECULAR":
                        material.Specular = ParseFloat(child.Value);
                        break;
                    case "SHININESS":
                        material.Shininess = ParseInt(child.Value);
                        break;
                    case "REFLECTIVE":
                        material.Reflective = ParseFloat(child.Value);
                        break;
                    case "TRANSPARENCY":
                        material.Transparency = ParseFloat(child.Value);
                        break;
                    case "REFRACTIVE-INDEX":
                        material.RefractiveIndex = ParseFloat(child.Value);
                        break;
                    case "COLOR":
                        material.Pattern = new SolidPattern(ParseColor(child.Value));
                        break;
                    case "PATTERN":
                        material.Pattern = ParsePattern(child.Value);
                        break;
                    default:
                        throw new Exception($"Material attribute {key} not supported (line {child.Key.Start.Line})");
                }
            }

            return material;
        }

        private Pattern ParsePattern(YamlNode node)
        {
            string type = null;
            var colors = new [] { Color.White, Color.Black };
            var transform = Matrix4x4.Identity();
            
            string[] supportedTypes = { "CHECKERS", "CHECKER", "STRIPES" };
            
            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();
                switch (key)
                {
                    case "TYPE":
                        type = ((YamlScalarNode) child.Value).Value.ToUpperInvariant();
                        if (!supportedTypes.Contains(type))
                        {
                            throw new Exception($"Pattern type {type} not supported (line {child.Key.Start.Line})");
                        }
                        break;
                    case "COLORS":
                        colors = ParseColors(child.Value);
                        break;
                    case "TRANSFORM":
                        transform = ParseTransform(child.Value);
                        break;
                    default:
                        throw new Exception($"Pattern attribute {key} not supported (line {child.Key.Start.Line})");
                }
            }

            switch (type)
            {
                case "CHECKERS":
                case "CHECKER":
                    return new CheckeredPattern(transform, new SolidPattern(colors[0]), new SolidPattern(colors[1]));
                case "STRIPES":
                    return new StripePattern(transform, new SolidPattern(colors[0]), new SolidPattern(colors[1]));
                default:
                    throw new Exception($"Pattern type {type} not supported (line {node.Start.Line})");
            }
        }

        private static Color[] ParseColors(YamlNode node)
        {
            var colorNodes = (YamlSequenceNode) node;
            var colors = new List<Color>(colorNodes.Children.Count);

            foreach (var colorNode in colorNodes)
            {
                colors.Add(ParseColor(colorNode));
            }

            return colors.ToArray();
        }
    }
}