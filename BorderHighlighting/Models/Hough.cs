using System;
using System.Collections.Generic;
using BorderHighlighting.Common;

namespace BorderHighlighting.Models;

public class Hough
{
    public struct Line
    {
        public double? K;
        public double B;
        public double X;

        public Line(double? k, double b, double x)
        {
            this.K = k;
            this.B = b;
            this.X = x;
        }
    }

    public List<Line> FindLines(Bitmap cannyImage, int threshold = 100)
    {
        const double pi = Math.PI;
        const int delta = 1;

        int width = cannyImage.Width;
        int height = cannyImage.Height;
        int d = (int) (Math.Round(Math.Sqrt(width * width + height * height)) + 1);
        //int[,] accumulate = new int[cannyImage.Width, cannyImage.Height];
        
        int[,] accumulate = new int[2*d + 1, 181];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (cannyImage.GetColor(i, j).I != 255)
                {
                    continue;
                }

                for (int th = 0; th <= 180; th += delta)
                {
                    double rad = (pi / 180.0) * th;
                    var (sin, cos) = Math.SinCos(rad);
                    int p = (int)Math.Round(i * cos + j * sin);
                    accumulate[p + d, th] += 1;
                }
            }
        }

        List<Line> lines = new List<Line>();

        for (int p = -d; p <= d; p++)
        {
            for (int th = 0; th <= 180; th++)
            {
                if (accumulate[d + p, th]  >= threshold)
                {
                    double rad = (pi / 180.0) * th;
                    var (sin, cos) = Math.SinCos(rad);
                    double x0 = (cos * p);
                    double y0 = (sin * p);

                    double x1 = (x0 + 1000 * (-sin));
                    double y1 = (y0 + 1000 * cos);
                    
                    double x2 = (x0 - 1000 * (-sin));
                    double y2 = (y0 - 1000 * cos);

                    double? k = null;
                    double b = y1;
                    if (Math.Abs(x1 - x2) > Double.Epsilon)
                    {
                        k = (y1 - y2) / (x1 - x2);
                        b = y1 - (x1 * (y1 - y2)) / (x1 - x2);
                    }

                    lines.Add(new Line(k, b, x0));
                }
            }
        }

        return lines;
    }
}
