using Accord.Math;

namespace RayTracer
{
    public class Triangle : GeometryObject
    {
        public Vector3 A { get; set; }
        public Vector3 B { get; set; }
        public Vector3 C { get; set; }
        public Vector3 FaceNormal { get; set; }

        //Note the counter-clockwise order
        public Triangle(Vector3 a, Vector3 c, Vector3 b, ObjectProperties objProps, Matrix4x4 transform)
            : base(objProps, transform)
        {
            A = Helpers.TransformVector(a, Transform);
            B = Helpers.TransformVector(b, Transform);
            C = Helpers.TransformVector(c, Transform);
            
            CalculateNormal();
        }

        private void CalculateNormal()
        {
            var vec = Vector3.Cross(C - A, B - A);
            vec.Normalize();
            FaceNormal = vec;
        }
    }
}