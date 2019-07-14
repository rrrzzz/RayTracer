using System;

namespace RayTracer
{
    public static class Helpers
    {
        public static float ToRadians(float degrees) => (float)(degrees * Math.PI / 180);
        public static float ToRadians(double degrees) => (float)(degrees * Math.PI / 180);

        public static float ToDegrees(float radians) => (float)(radians * 180 / Math.PI);
        public static float ToDegrees(double radians) => (float)(radians * 180 / Math.PI);

        public static float GetHalfFovInRad(float fovDegrees) => ToRadians(fovDegrees) * 0.5f;

    }
}