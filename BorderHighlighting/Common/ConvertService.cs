using System.IO;
using System.Windows.Media.Imaging;
using OpenCV;

namespace BorderHighlighting.Common;


public static class ConvertService
{
    public static BitmapImage WritableBitmapToBitmapImage(WriteableBitmap writeableBitmap, BitmapEncoder encoder)
    {
        var image = new BitmapImage();
        using var stream = new MemoryStream();
        encoder.Frames.Clear();
        encoder.Frames.Add(BitmapFrame.Create(writeableBitmap));
        encoder.Save(stream);
            
        image.BeginInit();
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.StreamSource = stream;
        image.EndInit();
        image.Freeze();

        return image;
    }

    public static ImageData BitmapToImageData(Bitmap bitmap)
    {
        return new ImageData(bitmap.Data, bitmap.Width, bitmap.Height, ImageData.ChannelsType.Brga);
    }

    public static Bitmap ToGrayscale(Bitmap bitmap)
    {
        var result = new Bitmap(bitmap);

        for (int i = 0; i < result.Width; i++)
        {
            for (int j = 0; j < result.Height; j++)
            {
                var color = bitmap.GetColor(i, j);
                var intensity = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
                result.SetColor(i,j, new Color((byte)intensity));
            }
        }

        return result;
    }
}
