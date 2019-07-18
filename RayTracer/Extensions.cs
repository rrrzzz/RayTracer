using System;
using Accord.Math;


namespace RayTracer
{
    public static class Extensions {
 
        public static float Map (this float value, float from1, float to1, float from2, float to2) {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        public static Vector3 Normalization(this Vector3 value)
        {
            var norm = (float) System.Math.Sqrt((double) value.X * (double) value.X + (double) value.Y * (double) value.Y + (double) value.Z * (double) value.Z);
            if (Math.Abs(norm) < Globals.Delta) return value;
            return value / norm;
        }
   
    }
}