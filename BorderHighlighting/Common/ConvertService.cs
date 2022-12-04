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
}
