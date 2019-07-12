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
            Random rand = new Random();

            var matStack = new Stack<Matrix4x4>();
            matStack.Push(Matrix4x4.Identity);
            
            var matTest = new Matrix4x4(2,0,0,0,0,2,0,0,0,0,2,0,0,0,0,2);
            
            matStack.Push(matTest);

            var mat = matStack.Peek();

            RightMultiplyTopStack(2, matStack);
            
            

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
        
        private static void RightMultiplyTopStack(int transformMatrix, Stack<Matrix4x4> stack)
        {
            var topMat = stack.Pop();
            topMat *= transformMatrix;
            stack.Push(topMat);
        }
    }
}