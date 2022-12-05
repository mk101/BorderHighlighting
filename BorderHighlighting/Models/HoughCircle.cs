using System;
using System.Collections.Generic;
using BorderHighlighting.Common;

namespace BorderHighlighting.Models;

public class HoughCircle
{
    public struct Circle
    {
        public double X0;
        public double Y0;
        public double R;

        public Circle(double x0, double y0, double r)
        {
            this.X0 = x0;
            this.Y0 = y0;
            this.R = r;
        }
    }

    public List<Circle> FindCircles(Bitmap cannyImage, int threshold, int minR = 50, int maxR = 180)
    {
        const int delta = 1;

        int width = cannyImage.Width;
        int height = cannyImage.Height;

        int[,,] accumulate = new int[width, height, maxR];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (cannyImage.GetColor(i, j).I != 255)
                {
                    continue;
                }
                for (int x0 = minR; x0 < width - minR; x0 += delta)
                {
                    for (int y0 = minR; y0 < height - minR; y0 += delta)
                    {
                        int r = (int)Math.Sqrt((i - x0) * (i - x0) + (j - y0) * (j - y0));
                        if (r > minR && r < maxR)
                        {
                            accumulate[x0, y0, r] += 1;
                        }
                    }
                }


            }
        }

        List<Circle> circles = new List<Circle>();
        for (int x0 = 0; x0 < width; x0++)
        {
            for (int y0 = 0; y0 < height; y0++)
            {
                for (int r = 0; r < maxR; r++)
                {
                    if (accumulate[x0, y0, r] >= threshold)
                    {
                        circles.Add(new Circle(x0, y0, r));
                    }

                }
            }
        }

        return circles;
    }
}
