using System.Drawing;
using System.Numerics;

namespace RayTracer
{
    public struct Light
    {
        public Light(Vector4 coord, Vector3 col, float[] attenuation)
        {
            Coordinates = coord;
            Color = col;
            Attenuation = attenuation;
        }
        
        public Vector4 Coordinates { get; }
        public Vector3 Color { get; }
        public float[] Attenuation { get; }
    }
}