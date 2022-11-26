using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public abstract class FileService
{
    public BitmapImage Open(string fileName)
    {
        return new BitmapImage(new Uri(fileName));
    }

    public void Save(BitmapImage image, string fileName)
    {
        var encoder = GetEncoder();
        encoder.Frames.Add(BitmapFrame.Create(image));

        using var fileStream = new FileStream(fileName, FileMode.Create);
        encoder.Save(fileStream);
    }

    public abstract BitmapEncoder GetEncoder();
}
