using System;
using System.Collections.Generic;
using System.Numerics;

namespace RayTracer
{
    public static class Globals
    {
        public static int ImageWidth { get; set; }
        public static int ImageHeight { get; set; }
        public static int RecursionMaxDepth { get; set; }
        public static string OutputFilename { get; set; }
        public static float FovY { get; set; }
        public static int MaxVerts { get; set; }
        public static Vector3[] Vertices { get; set; }
        public static List<Triangle> Triangles { get; set; }
    }
}