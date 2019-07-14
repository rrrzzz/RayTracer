using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Numerics;

namespace RayTracer
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var testFilePath = @"D:\Docs\Courses\Computer graphics edx\testscenes\scene2.test";
            var parser = new Parser();
            parser.ReadFile(testFilePath);

            var x = 0;
            x *= 2;
        }
        
        private static void RightMultiplyTopStack(int transformMatrix, Stack<Matrix4x4> stack)
        {
            var topMat = stack.Pop();
            topMat *= transformMatrix;
            stack.Push(topMat);
        }

        private static void CreateImage()
        {
            var width = 1024;
            var height = 768;
            
            Bitmap p = new Bitmap(width,height);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    p.SetPixel(i, j, Color.FromArgb(255, 255,255, 255));
                }
            }
            
            p.Save(@"D:\Docs\Courses\Computer graphics edx\img.png", ImageFormat.Png);
        }
    }
}