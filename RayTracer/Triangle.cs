using System.Numerics;

namespace RayTracer
{
    public struct Triangle
    {
        //Note the counter-clockwise order
        public Triangle(Vector3 a, Vector3 c, Vector3 b)
        {
            A = a;
            B = b;
            C = c;
            FaceNormal = Vector3.Cross(C - A, B - A);
        }
        
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }
        public Vector3 FaceNormal { get; set; }
    }
}