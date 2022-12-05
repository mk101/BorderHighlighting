using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public class PngFileService : FileService
{
    public override BitmapEncoder GetEncoder()
    {
        return new PngBitmapEncoder();
    }
}
