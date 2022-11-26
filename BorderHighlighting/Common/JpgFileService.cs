using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public class JpgFileService : FileService
{
    public JpgFileService()
    {
        _encoder = new JpegBitmapEncoder();
    }

    public override BitmapEncoder GetEncoder()
    {
        return _encoder;
    }

    private readonly BitmapEncoder _encoder;
}
