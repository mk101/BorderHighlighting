using System;
using BorderHighlighting.Common;

namespace BorderHighlighting.Models;

public class CannyService
{
    public static Bitmap Processing(Bitmap sourceImage)
    {
        var image = ReductionNoise(sourceImage);
        return image;
    }

    private static Bitmap ReductionNoise(Bitmap sourceImage)
    {
        var image = new Bitmap(sourceImage);
        
        for (var y = 2; y < sourceImage.Height-2; y++) {
            for (var x = 2; x < sourceImage.Width-2; x++)
            {

                double intensity = 0;
                for (var i = 0; i < GaussianKernel.GetLength(0); i++) {
                    for (var j = 0; j < GaussianKernel.GetLength(1); j++) {
                        intensity += sourceImage.GetColor(x+j-2, y+i-2).I;
                    }
                }

                image.SetColor(x, y, new Color(Utils.Clamp(intensity/GaussianKernel.Length)));
            }
        }
        
        return image;
    }

    private static readonly double[,] GaussianKernel =
    {
        {2.0/159, 4.0/159, 5.0/159, 4.0/159, 2.0/159},
        {4.0/159, 9.0/159, 12.0/159, 9.0/159, 4.0/159},
        {5.0/159, 12.0/159, 15.0/159, 12.0/159, 5.0/159},
        {4.0/159, 9.0/159, 12.0/159, 9.0/159, 4.0/159},
        {2.0/159, 4.0/159, 5.0/159, 4.0/159, 2.0/159},
    };
}