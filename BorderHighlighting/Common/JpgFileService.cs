using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public class JpgFileService : FileService
{
    public override BitmapEncoder GetEncoder()
    {
        return new JpegBitmapEncoder();
    }
    
}
