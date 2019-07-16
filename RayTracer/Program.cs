using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using Accord.Math;
using static RayTracer.Globals;

namespace RayTracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var testFilePath = @"D:\Docs\Courses\Computer graphics edx\testscenes\scene1.test";
            var parser = new Parser();
            parser.ReadFile(testFilePath);

            Ray.InitializeCoordinateFrame();
            
            
        }

        private static void CreateImage()
        {
            
            
            
            var p = new Bitmap(ImageWidth,ImageHeight);

            for (var i = 0; i < ImageWidth; i++)
            {
                for (var j = 0; j < ImageHeight; j++)
                {
                    var ray = new Ray(i + 0.5f, j + 0.5f);
                    var intersectionObject = new Intersection(ray);
                    
                    p.SetPixel(i, j, Color.FromArgb(255, 255,255, 255));
                }
            }
            
            p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
        }
    }
}