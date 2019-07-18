using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Accord.Math;
using static RayTracer.Globals;
using static RayTracer.Transform;

namespace RayTracer
{
    public class Parser
    {
        private float[] CmdValues { get; set; }

        public void ReadFile(string path)
        {
            var transformStack = new Stack<Matrix4x4>();
            transformStack.Push(Matrix4x4.Identity);
            
            var contents = File.ReadLines(path);
            var vertCount = 0;
            
            foreach (var line in contents)
            {
                string[] splitLine;
                ObjectProperties objProps;
                Vector4 lightPos;
                Vector3 lightCol;
                
                if (line.StartsWith("#") || line == string.Empty) continue;
                else splitLine = line.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);
                var cmd = splitLine[0];
                

                switch (cmd)
                {
                    case "size":
                        ImageWidth = int.Parse(splitLine[1]);
                        ImageHeight = int.Parse(splitLine[2]);
                        SetAspectRatio();
                        break;
                    
                    case "maxdepth":
                        RecursionMaxDepth = int.Parse(splitLine[1]);
                        break;
                    
                    case "maxverts":
                        Vertices = new Vector3[int.Parse(splitLine[1])];
                        break;
                    
                    case "vertex":
                        GetFloatCmdValues(splitLine);                        
                        Vertices[vertCount++] = Get3dVectorFromFirstValues();
                        break;
                    
                    case "tri":
                        GetFloatCmdValues(splitLine);
                        var vert1 = Vertices[(int)CmdValues[0]];
                        var vert2 = Vertices[(int)CmdValues[1]];
                        var vert3 = Vertices[(int)CmdValues[2]];
                        objProps = CreateObjectProperties();
                        Triangles.Add(new Triangle(vert1, vert2, vert3, objProps, transformStack.Peek()));
                        break;
                    
                    case "sphere":
                        GetFloatCmdValues(splitLine);
                        objProps = CreateObjectProperties();
                        var spherePos = new Vector3(CmdValues[0], CmdValues[1], CmdValues[2]);
                        Spheres.Add(new Sphere(spherePos, CmdValues[3], objProps, transformStack.Peek()));
                        break;
                        
                    case "output":
                        OutputFilename = splitLine[1];
                        break;
                    
                    case "camera":
                        GetFloatCmdValues(splitLine);
                        EyeInit = new Vector3(CmdValues[0], CmdValues[1], CmdValues[2]);
                        LookAtPoint = new Vector3(CmdValues[3], CmdValues[4], CmdValues[5]);
                        UpInit = new Vector3(CmdValues[6], CmdValues[7], CmdValues[8]);
                        FovYRad = Transform.ToRadians(CmdValues[9]);
                        SetFovX();
                        ModelView = SetLookAtMatrix();
                        break;
                    
                    case "attenuation":
                        GetFloatCmdValues(splitLine);
                        Attenuation = new[] {CmdValues[0], CmdValues[1], CmdValues[2]};
                        break;
                    
                    case "ambient":
                        GetFloatCmdValues(splitLine);
                        Ambient = Get3dVectorFromFirstValues();
                        break;
                    
                    case "diffuse":
                        GetFloatCmdValues(splitLine);
                        Diffuse = Get3dVectorFromFirstValues();
                        break;
                    
                    case "specular":
                        GetFloatCmdValues(splitLine);
                        Specular = Get3dVectorFromFirstValues();
                        break;
                    
                    case "emission":
                        GetFloatCmdValues(splitLine);
                        Emission = Get3dVectorFromFirstValues();
                        break;
                    
                    case "shininess":
                        Shininess = float.Parse(splitLine[1]);
                        break;
                    
                    case "directional":
                        GetFloatCmdValues(splitLine);
                        lightPos = new Vector4(CmdValues[0], CmdValues[1], CmdValues[2], 0);
                        lightCol = new Vector3(CmdValues[3], CmdValues[4], CmdValues[5]);
                        Lights.Add(new Light(lightPos, lightCol, Attenuation));
                        break;
                    
                    case "point":
                        GetFloatCmdValues(splitLine);
                        lightPos = new Vector4(CmdValues[0], CmdValues[1], CmdValues[2], 1);
                        lightCol = new Vector3(CmdValues[3], CmdValues[4], CmdValues[5]);
                        Lights.Add(new Light(lightPos, lightCol, Attenuation));
                        break;
                    
                    case "translate":
                        GetFloatCmdValues(splitLine);
                        var translateMat = GetTranslationMatrix(Get3dVectorFromFirstValues());
                        RightMultiplyTopStack(translateMat, transformStack);
                        break;
                    
                    case "rotate":
                        GetFloatCmdValues(splitLine);
                        var angle = CmdValues[3];
                        var rotMat = GetRotationMatrix(Get3dVectorFromFirstValues(), angle);
                        RightMultiplyTopStack(rotMat, transformStack);
                        break;
                    
                    case "scale":
                        GetFloatCmdValues(splitLine);
                        var scaleMat = GetScaleMatrix(Get3dVectorFromFirstValues());
                        RightMultiplyTopStack(scaleMat, transformStack);
                        break;
                    
                    case "pushTransform":
                        transformStack.Push(transformStack.Peek());
                        break;
                    
                    case "popTransform":
                        transformStack.Pop();
                        break;
                    
                    default:
                        if (cmd == "maxvertnorms" ||
                            cmd == "vertexnormal" || cmd == "trinormal") break;
                        throw new InvalidEnumArgumentException($"There is no such command as {cmd}");
                }
            }
        }

        private void RightMultiplyTopStack(Matrix4x4 transformMatrix, Stack<Matrix4x4> stack)
        {
            var topMat = stack.Pop();
            topMat *= transformMatrix;
            stack.Push(topMat);
        }
        
        private ObjectProperties CreateObjectProperties() => 
            new ObjectProperties(Shininess, Ambient, Diffuse, Specular, Emission);
        
        private Vector3 Get3dVectorFromFirstValues() => 
            new Vector3(CmdValues[0], CmdValues[1], CmdValues[2]);
        
        private void GetFloatCmdValues(IEnumerable<string> input) => 
            CmdValues = input.Skip(1).Select(float.Parse).ToArray();
        
        private void SetAspectRatio()
        {
            if (ImageWidth == 0 || ImageHeight == 0) 
                throw new ArgumentException("ImageHeight or ImageWidth haven't been initialized");

            AspectRatio = (float)ImageWidth / ImageHeight;
        }

        private void SetFovX()
        {
            if (FovYRad < 0.1 || AspectRatio < 0.1) 
                throw new ArgumentException("FovY or AspectRatio haven't been initialized");

            FovXRad = (float)(2 * Math.Atan(Math.Tan(FovYRad * 0.5) * AspectRatio));
        }
    }
}