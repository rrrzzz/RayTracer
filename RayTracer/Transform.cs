using System;
using Accord.Math;
using static RayTracer.Globals;

namespace RayTracer
{
    public static class Transform
    {
        public static Matrix4x4 GetTranslationMatrix(Vector3 v)
        {
            return new Matrix4x4{ V00 = 1, V01 = 0, V02 = 0, V03 = v.X,
                                  V10 = 0, V11 = 1, V12 = 0, V13 = v.Y,
                                  V20 = 0, V21 = 0, V22 = 1, V23 = v.Z,
                                  V30 = 0, V31 = 0, V32 = 0, V33 = 1 };
        }

        public static Matrix4x4 GetRotationMatrix(Vector3 axis, float angle)
        {
            var degRad = ToRadians(angle);
            var I = Matrix3x3.Identity;
            
            axis.Normalize();
            
            var x = axis.X;
            var y = axis.Y;
            var z = axis.Z;

            var axisMulAxisTransposed = new Matrix3x3(){V00 = x * x, V01 = x * y, V02 = x * z,
                                                        V10 = y * x, V11 = y * y, V12 = y * z,
                                                        V20 = z * x, V21 = z * y, V22 = z * z};
            

            var crossProdMatrix = new Matrix3x3(){V00 = 0, V01 = -z, V02 = y,
                                                  V10 = z, V11 = 0, V12 = -x,
                                                  V20 = -y, V21 = x, V22 = 0};
            
            var rotMat3X3 = I * (float)Math.Cos(degRad) + axisMulAxisTransposed * (float)(1 - Math.Cos(degRad)) + 
                      crossProdMatrix * (float)Math.Sin(degRad);
            
            var ret = Matrix4x4.CreateFromRotation(rotMat3X3);
            return Matrix4x4.CreateFromRotation(rotMat3X3);
        }

        public static Matrix4x4 GetScaleMatrix(Vector3 v)
        {
            return Matrix4x4.CreateDiagonal(new Vector4(v.X, v.Y, v.Z, 1));
        }

        public static Vector3[] ConstructCoordinateFrame()
        {
            var z = (EyeInit - LookAtPoint).Normalization();
            var x = Vector3.Cross(UpInit, z).Normalization();
            var y = Vector3.Cross(z, x);
            
            return new []{x,y,z};
        }
        
        public static Matrix4x4 SetLookAtMatrix()
        {
            var z = EyeInit.Normalization();
            var x = Vector3.Cross(UpInit, z).Normalization();
            var y = Vector3.Cross(z, x);

            return new Matrix4x4 {V00 = x.X, V01 = x.Y, V02 = x.Z, V03 = Vector3.Dot(x*-1, EyeInit),
                V10 = y.X, V11 = y.Y, V12 = y.Z, V13 = Vector3.Dot(y*-1, EyeInit),
                V20 = z.X, V21 = z.Y, V22 = z.Z, V23 = Vector3.Dot(z*-1, EyeInit),
                V30 = 0, V31 = 0, V32 = 0, V33 = 1};
        }
        
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
            
            var ret = new Matrix4x4
            {
                V00 = inverse[0, 0], V01 = inverse[0, 1], V02 = inverse[0, 2], V03 = inverse[0, 3],
                V10 = inverse[1, 0], V11 = inverse[1, 1], V12 = inverse[1, 2], V13 = inverse[1, 3],
                V20 = inverse[2, 0], V21 = inverse[2, 1], V22 = inverse[2, 2], V23 = inverse[2, 3],
                V30 = inverse[3, 0], V31 = inverse[3, 1], V32 = inverse[3, 2], V33 = inverse[3, 3]
            };
            
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

        public static Vector3 ConvertToVector3(Vector4 v)
        {
            return Math.Abs(v.W) < Globals.Delta ? 
                new Vector3(v.X, v.Y, v.Z) : new Vector3(v.X / v.W, v.Y / v.W, v.Z / v.W);
        }
    }
}