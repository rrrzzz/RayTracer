using System;
using Accord.Math;

namespace RayTracer
{
    public static class Helpers
    {
        public static float ToRadians(float degrees) => (float)(degrees * Math.PI / 180);
        public static float ToRadians(double degrees) => (float)(degrees * Math.PI / 180);

        public static float ToDegrees(float radians) => (float)(radians * 180 / Math.PI);
        public static float ToDegrees(double radians) => (float)(radians * 180 / Math.PI);

        public static float GetHalfFovInRad(float fovDegrees) => ToRadians(fovDegrees) * 0.5f;

        public static Matrix4x4 GetInverseMatrix4X4(Matrix4x4 mat)
        {
            float[,] matrix =
            {
                {mat.V00, mat.V01, mat.V02, mat.V03},
                {mat.V10, mat.V11, mat.V12, mat.V13},
                {mat.V20, mat.V21, mat.V22, mat.V23},
                {mat.V30, mat.V31, mat.V32, mat.V33}
            };

            var inverse = matrix.Inverse();
            
            return new Matrix4x4
            {
                V00 = inverse[0, 0], V01 = inverse[0, 1], V02 = inverse[0, 2], V03 = inverse[0, 3],
                V10 = inverse[1, 0], V11 = inverse[1, 1], V12 = inverse[1, 2], V13 = inverse[1, 3],
                V20 = inverse[2, 0], V21 = inverse[2, 1], V22 = inverse[2, 2], V23 = inverse[2, 3],
                V30 = inverse[3, 0], V31 = inverse[3, 1], V32 = inverse[3, 2], V33 = inverse[3, 3]
            };
        }
        
        public static Vector3 TransformVector(Vector3 v, Matrix4x4 transformMat)
        {
            var homogeneousPos = transformMat * new Vector4(v.X, v.Y, v.Z, 1);
            var dehomogenizedPosition = homogeneousPos / homogeneousPos.W;
            return new Vector3(dehomogenizedPosition.X, dehomogenizedPosition.Y, dehomogenizedPosition.Z);
        }
        
        public static Vector3 TransformZeroWVector(Vector3 v, Matrix4x4 transformMat)
        {
            var homogeneousPos = transformMat * new Vector4(v.X, v.Y, v.Z, 0);
            var dehomogenizedPosition = homogeneousPos / homogeneousPos.W;
            return new Vector3(dehomogenizedPosition.X, dehomogenizedPosition.Y, dehomogenizedPosition.Z);
        }
    }
}