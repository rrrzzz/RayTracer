using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace RayTracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Random rand = new Random();

            var width = 1024;
            var height = 768;
            
            Bitmap p = new Bitmap(width,height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    p.SetPixel(i, j, Color.FromArgb(255, rand.Next(255), rand.Next(255), rand.Next(128)));
                }
            }
            
            p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
        }
    }
}