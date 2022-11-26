using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public class PngFileService : FileService
{
    public PngFileService()
    {
        _encoder = new PngBitmapEncoder();
    }

    public override BitmapEncoder GetEncoder()
    {
        return _encoder;
    }

    private readonly BitmapEncoder _encoder;
}
