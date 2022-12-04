using System;
using BorderHighlighting.Common;

namespace BorderHighlighting.Models;

public class CannyService
{
    public static Bitmap Processing(Bitmap sourceImage)
    {
        var image = ReductionNoise(sourceImage);
        var imageWithGradient = CobelService.Processing(image);
        image = NonMaximumSuppression(imageWithGradient.Image, imageWithGradient.Gradient);
        
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

    private static Bitmap NonMaximumSuppression(Bitmap sourceImage, double[,] gradient)
    {
        var image = new Bitmap(sourceImage);

        for (var y = 1; y < sourceImage.Height - 1; y++) {
            for (var x = 1; x < sourceImage.Width - 1; x++) {
                var baseIntensity = sourceImage.GetColor(x, y).I;

                byte intensity1, intensity2;
                switch (gradient[y,x])
                {
                    case < 22.5:
                    case >= 157.5 and <= 180:
                        intensity1 = sourceImage.GetColor(x, y+1).I;
                        intensity2 = sourceImage.GetColor(x, y-1).I;
                        break;
                    case >= 22.5 and < 67.5:
                        intensity1 = sourceImage.GetColor(x+1, y-1).I;
                        intensity2 = sourceImage.GetColor(x-1, y+1).I;
                        break;
                    case >= 67.5 and < 112.5:
                        intensity1 = sourceImage.GetColor(x + 1, y).I;
                        intensity2 = sourceImage.GetColor(x - 1, y).I;
                        break;
                    default:
                        intensity1 = sourceImage.GetColor(x-1, y-1).I;
                        intensity2 = sourceImage.GetColor(x+1, y+1).I;
                        break;
                }

                if (baseIntensity >= intensity1 && baseIntensity > intensity2) {
                    image.SetColor(x, y, new Color(baseIntensity));
                }else {
                    image.SetColor(x, y, new Color(0));
                }
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