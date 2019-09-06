using System;
using System.Collections.Generic;
using Accord.Math;
using RayTracer.GeometryObjects;

namespace RayTracer
{
    public static class Globals
    {
        public const float Err = 1e-12f;
        public static int ImageWidth { get; set; }
        public static int ImageHeight { get; set; }
        public static float AspectRatio { get; set; }
        public static int RecursionMaxDepth { get; set; } = 5;
        public static string OutputFilename { get; set; }
        public static float FovYRad { get; set; }
        public static float FovXRad { get; set; }
        public static Vector3[] Vertices { get; set; }
        public static List<Triangle> Triangles { get; set; } = new List<Triangle>();
        public static List<Sphere> Spheres { get; } = new List<Sphere>();
        public static float Shininess { get; set; }
        public static Vector3 Ambient { get; set; } = new Vector3(.2f,.2f,.2f);
        public static Vector3 Diffuse { get; set; }
        public static Vector3 Specular { get; set; }
        public static Vector3 Emission { get; set; } = new Vector3(0,0,0);
        public static Vector3 EyeInit { get; set; }
        public static Vector3 UpInit { get; set; }
        public static Vector3 LookAtPoint { get; set; }
        public static float[] Attenuation { get; set; } = {1, 0, 0};
        public static List<Light> Lights { get; } = new List<Light>();
        public const float Delta = 0.000001f;
        public static Matrix4x4 ModelView { get; set; }
    }
}