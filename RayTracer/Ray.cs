using System;
using System.Threading;
using Accord.Math;
using static RayTracer.Globals;

namespace RayTracer
{
    public class Ray
    {
        private static Vector3 _xAxis;
        private static Vector3 _yAxis;
        private static Vector3 _zAxis;
        
        public Ray(float pixelX, float pixelY)
        {
            GetDirection(pixelX, pixelY);
            Origin = EyeInit;
        }

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
        
        public Vector3 Direction { get; private set; }
        public Vector3 Origin { get; }
        
        public static void InitializeCoordinateFrame()
        {
            var axes = Transform.ConstructCoordinateFrame(EyeInit, LookAtPoint, UpInit);
            _xAxis = axes[0];
            _yAxis = axes[1];
            _zAxis = axes[2];
        }

        private void GetDirection(float pixelX, float pixelY)
        {
            var halfImageHeight = ImageHeight * .5;
            var halfImageWidth = ImageWidth * .5;
            
            var xDirectionMultiplier = (float)(Math.Tan(FovXRad * .5) * (pixelX - halfImageWidth) / halfImageWidth);
            var yDirectionMultiplier = (float)(Math.Tan(FovYRad * .5) * (halfImageHeight - pixelY) / halfImageHeight);

            var dirVec = _xAxis * xDirectionMultiplier +
                         _yAxis * yDirectionMultiplier - _zAxis;
            dirVec.Normalize();
            
            Direction = EyeInit + dirVec;
        }
    }
}