using System;
using Accord.Math;
using Matrix4x4 = System.Numerics.Matrix4x4;
using Vector3 = System.Numerics.Vector3;


namespace RayTracer
{
    public static class Transform
    {
        public static Matrix4x4 GetTranslationMatrix(Vector3 v)
        {
            return new Matrix4x4(0, 0, 0, v.X,
                                 0, 0, 0, v.Y,
                                 0, 0, 0, v.Z,
                                 0, 0, 0,  1);
        }

        public static Matrix4x4 GetRotationMatrix(Vector3 axis, float angle)
        {
            var degRad = Helpers.ToRadians(angle);
            var I = Matrix3x3.Identity;
            
            var axisNormy = Vector3.Normalize(axis);
            
            var x = axisNormy.X;
            var y = axisNormy.Y;
            var z = axisNormy.Z;

            var axisMulAxisTransposed = new Matrix3x3(){V00 = x * x, V01 = x * y, V02 = x * z,
                                                        V10 = y * x, V11 = y * y, V12 = y * z,
                                                        V20 = z * x, V21 = z * y, V22 = z * z};
            

            var crossProdMatrix = new Matrix3x3(){V00 = 0, V01 = -z, V02 = y,
                                                  V10 = z, V11 = 0, V12 = -x,
                                                  V20 = -y, V21 = x, V22 = 0};
            
            //I * cos(degRad) + (1 - cos(degRad)) * axisMulAxisTransposed + sin(degRad) * crossProdMatrix;
            var rotMat3X3 = I * (float)Math.Cos(degRad) + axisMulAxisTransposed * (float)(1 - Math.Cos(degRad)) + 
                      crossProdMatrix * (float)Math.Sin(degRad);

            var result = new Matrix4x4(rotMat3X3.V00, rotMat3X3.V01, rotMat3X3.V02, 0,
                                       rotMat3X3.V10, rotMat3X3.V11, rotMat3X3.V12, 0,
                                       rotMat3X3.V20, rotMat3X3.V21, rotMat3X3.V22, 0,
                                          0,       0,       0,    1);
            return result;

        }

        public static Matrix4x4 GetScaleMatrix(Vector3 v)
        {
            return new Matrix4x4(v.X,  0,   0,   0,
                                  0,   v.Y, 0,   0,
                                  0,   0,  v.Z,  0,
                                  0,   0,   0,   1);
        }

        public static Matrix4x4 ConstructCoordinateFrame(Vector3 eyePos, Vector3 lookAtPos, Vector3 up)
        {
            var z = Vector3.Normalize(eyePos - lookAtPos);
            var x = Vector3.Normalize(Vector3.Cross(up, z));
            var y = Vector3.Cross(z, x);
            
            return new Matrix4x4(x.X, x.Y, x.Z, 0,
                                 y.X, y.Y, y.Z, 0,
                                 z.X, z.Y, z.Z, 0,
                                  0,   0,   0,  1);
        }
    }
}