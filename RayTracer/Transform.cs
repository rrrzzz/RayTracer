using System;
using Accord.Math;



namespace RayTracer
{
    public static class Transform
    {
        public static Matrix4x4 GetTranslationMatrix(Vector3 v)
        {
            return new Matrix4x4{ V00 = 0, V01 = 0, V02 = 0, V03 = v.X,
                                  V10 = 0, V11 = 0, V12 = 0, V13 = v.Y,
                                  V20 = 0, V21 = 0, V22 = 0, V23 = v.Z,
                                  V30 = 0, V31 = 0, V32 = 0, V33 = 1 };
        }

        public static Matrix4x4 GetRotationMatrix(Vector3 axis, float angle)
        {
            var degRad = Helpers.ToRadians(angle);
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
            
            return Matrix4x4.CreateFromRotation(rotMat3X3);
        }

        public static Matrix4x4 GetScaleMatrix(Vector3 v)
        {
            return Matrix4x4.CreateDiagonal(new Vector4(v.X, v.Y, v.Z, 1));
        }

        
        // maybe change eyePos - lookAtPos to just eyePos (in HW2 and HW1 it is written that way)
        public static Vector3[] ConstructCoordinateFrame(Vector3 eyePos, Vector3 lookAtPos, Vector3 up)
        {
            var z = eyePos - lookAtPos;
            z.Normalize();
            var x = Vector3.Cross(up, z);
            x.Normalize();
            var y = Vector3.Cross(z, x);
            
            return new []{x,y,z};
        }
    }
}