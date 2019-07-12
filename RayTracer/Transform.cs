using System;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;


namespace RayTracer
{
    public static class Transform
    {
        public static Matrix4x4 GetTranslationMatrix(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4 GetRotationMatrix(Vector3 axis, float angle)
        {
            throw new NotImplementedException();
        }

        public static Matrix4x4 GetScaleMatrix(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public static Vector4 ConstructCoordinateFrame(Vector3 eyePos, Vector3 lookAtPos, Vector3 up)
        {
            throw new NotImplementedException();
        }
    }
}