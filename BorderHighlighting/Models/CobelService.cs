using System;
using BorderHighlighting.Common;

namespace BorderHighlighting.Models;

public static class CobelService
{
    public static ImageWithGradient Processing(Bitmap sourceImage)
    {
        float[,] kernelX =
        {
            {-1, 0, 1},
            {-2, 0, 2},
            {-1, 0, 1}
        };
        
        float[,] kernelY =
        {
            {-1, -2, -1},
            {0, 0, 0},
            {1, 2, 1}
        };
        
        var image = new Bitmap(sourceImage);
        var gradient = new double[sourceImage.Height, sourceImage.Width];
        
        for (var y = 1; y < sourceImage.Height-1; y++) {
            for (var x = 1; x < sourceImage.Width-1; x++)
            {
                float intensityX = 0;
                float intensityY = 0;
                for (var i = 0; i < 3; i++) {
                    for (var j = 0; j < 3; j++) {
                        var xn = x + j - 1;
                        var yn = y + i - 1;
                        
                        intensityX += kernelX[i, j] * sourceImage.GetColor(xn, yn).I;
                        intensityY += kernelY[i, j] * sourceImage.GetColor(xn, yn).I;
                        gradient[yn, xn] = Math.Atan(intensityY/intensityX);
                    }
                }

                var intensity = Utils.Clamp(Math.Sqrt(intensityX * intensityX + intensityY * intensityY));
                image.SetColor(x, y, new Color(intensity, intensity, intensity));
            }
        }

        return new ImageWithGradient {
            Image = image,
            Gradient = gradient
        };
    }
}

public struct ImageWithGradient
{
    public Bitmap Image;
    public double[,] Gradient;
}