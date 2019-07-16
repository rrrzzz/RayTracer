using Accord.Math;

namespace RayTracer
{
    public struct ObjectProperties
    {
        public ObjectProperties(float shine, Vector3 amb, Vector3 diff, Vector3 spec,
            Vector3 emis)
        {
            Shininess = shine;
            Ambient = amb;
            Diffuse = diff;
            Specular = spec;
            Emission = emis;
        }
        
        public float Shininess { get; }
        public Vector3 Ambient { get; }
        public Vector3 Diffuse { get; }
        public Vector3 Specular { get; }
        public Vector3 Emission { get; }
    }
}