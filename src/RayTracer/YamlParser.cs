using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RayTracer.Lib;
using RayTracer.Lib.Patterns;
using YamlDotNet.RepresentationModel;

namespace RayTracer
{
    public class YamlParser
    {
        private Dictionary<string, Material> materials;
        
        public (World, Camera) LoadYamlFile(string file)
        {
            var world = new World();
            Camera camera = null;
            materials = new Dictionary<string, Material>();
            
            using var reader = new StreamReader(file);
            var yaml = new YamlStream();
            yaml.Load(reader);
            
            var mapping = (YamlSequenceNode)yaml.Documents[0].RootNode;

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
                                world.Shapes.Add(ParseShape(node));
                                break;
                            default:
                                throw new Exception($"Add {type} not supported");
                        }
                        break;
                    case "DEFINE":
                        var name = type;
                        type = type.Substring(type.LastIndexOf('-') + 1);
                        
                        switch (type)
                        {
                            case "MATERIAL":
                                materials.Add(name, ParseMaterial(node.AllNodes.ToArray()[4]));
                                break;
                            default:
                                throw new Exception($"Define {name} not supported");
                        }
                        break;
                    default:
                        throw new Exception($"Command {command} not supported");
                }
            }
            
            return (world, camera);
        }

        private Shape ParseShape(YamlNode node)
        {
            string type = null;
            var transform = Matrix4x4.Identity();
            var material = new Material();
            
            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();

                switch (key)
                {
                    case "ADD":
                        type = ((YamlScalarNode) child.Value).Value.ToUpperInvariant();
                        break;
                    case "TRANSFORM":
                        transform = ParseTransform(child.Value);
                        break;
                    case "MATERIAL":
                        material = ParseMaterial(child.Value);
                        break;
                    default:
                        throw new Exception($"Shape attribute {key} not supported");
                }
            }

            switch (type)
            {
                case "PLANE":
                    return new Plane(transform, material);
                case "SPHERE":
                    return new Sphere(transform, material);
                default:
                    throw new Exception($"Shape type {type} not supported");
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
                        throw new Exception($"Light attribute {key} not supported");
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
                        throw new Exception($"Camera attribute {key} not supported");
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
            var transforms = (YamlSequenceNode) node;

            foreach (var transformNode in transforms)
            {
                var type = ((YamlScalarNode) transformNode[0]).Value.ToUpperInvariant();
            
                switch (type)
                {
                    case "TRANSLATE":
                        transform *= Matrix4x4.Translation(ParseFloat(transformNode[1]), ParseFloat(transformNode[2]), ParseFloat(transformNode[3]));
                        break;
                    case "SCALE":
                        transform *= Matrix4x4.Scaling(ParseFloat(transformNode[1]), ParseFloat(transformNode[2]), ParseFloat(transformNode[3]));
                        break;
                    case "ROTATE-X":
                        transform *= Matrix4x4.RotationX(ParseFloat(transformNode[1]));
                        break;
                    case "ROTATE-Y":
                        transform *= Matrix4x4.RotationY(ParseFloat(transformNode[1]));
                        break;
                    case "ROTATE-Z":
                        transform *= Matrix4x4.RotationZ(ParseFloat(transformNode[1]));
                        break;
                    default:
                        throw new Exception($"Transform {type} not supported");
                }
            }

            return transform;
        }

        private Material ParseMaterial(YamlNode node)
        {
            if (node.NodeType == YamlNodeType.Scalar)
            {
                string key = ((YamlScalarNode) node).Value.ToUpperInvariant();
                return materials[key];
            }
            
            var ambient = 0.1f;
            var diffuse = 0.9f;
            var specular = 0.9f;
            var shininess = 200;
            var reflective = 0f;
            var transparency = 0f;
            var refractiveIndex = 1f;
            Pattern pattern = null;

            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();
                switch (key)
                {
                    case "AMBIENT":
                        ambient = ParseFloat(child.Value);
                        break;
                    case "DIFFUSE":
                        diffuse = ParseFloat(child.Value);
                        break;
                    case "SPECULAR":
                        specular = ParseFloat(child.Value);
                        break;
                    case "SHININESS":
                        shininess = ParseInt(child.Value);
                        break;
                    case "REFLECTIVE":
                        reflective = ParseFloat(child.Value);
                        break;
                    case "TRANSPARENCY":
                        transparency = ParseFloat(child.Value);
                        break;
                    case "REFRACTIVE-INDEX":
                        refractiveIndex = ParseFloat(child.Value);
                        break;
                    case "COLOR":
                        pattern = new SolidPattern(ParseColor(child.Value));
                        break;
                    case "PATTERN":
                        pattern = ParsePattern(child.Value);
                        break;
                    default:
                        throw new Exception($"Material attribute {key} not supported");
                }
            }

            if (pattern == null)
            {
                return new Material(ambient, diffuse, specular,
                    shininess, reflective, transparency, refractiveIndex);
            }

            return new Material(pattern, ambient, diffuse, specular,
                shininess, reflective, transparency, refractiveIndex);
        }

        private Pattern ParsePattern(YamlNode node)
        {
            string type = null;
            Color[] colors = null;
            
            foreach (var child in (YamlMappingNode)node)
            {
                var key = ((YamlScalarNode) child.Key).Value.ToUpperInvariant();
                switch (key)
                {
                    case "TYPE":
                        type = ((YamlScalarNode) child.Value).Value.ToUpperInvariant();
                        break;
                    case "COLORS":
                        colors = ParseColors(child.Value);
                        break;
                    default:
                        throw new Exception($"Pattern attribute {key} not supported");
                }
            }

            switch (type)
            {
                case "CHECKERS":
                case "CHECKER":
                    return new CheckeredPattern(new SolidPattern(colors[0]), new SolidPattern(colors[1]));
                default:
                    throw new Exception($"Pattern type {type} not supported");
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