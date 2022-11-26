using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace BorderHighlighting.Common;

public class FileManager
{
    public const string Filter = "png files (*.png)|*.png|jpg files (*.jpg)|*.jpg";

    public BitmapEncoder? Encoder => _fileService?.GetEncoder();
    
    public BitmapImage? Open()
    {
        var ofd = new OpenFileDialog
        {
            Filter = Filter
        };

        bool? result = ofd.ShowDialog();
        if (result == false)
        {
            return null;
        }

        _fileService = Path.GetExtension(ofd.FileName) switch
        {
            ".png" => new PngFileService(),
            ".jpg" => new JpgFileService(),
            _ => throw new FileFormatException("Unknown file format")
        };

        return _fileService.Open(ofd.FileName);
    }

    public void Save(BitmapImage? bitmap)
    {
        if (bitmap is null)
        {
            return;
        }
        if (_fileService is null)
        {
            return;
        }
        
        var sfd = new SaveFileDialog()
        {
            Filter = Filter
        };

        bool? result = sfd.ShowDialog();
        if (result == false)
        {
            return;
        }
        
        _fileService.Save(bitmap, sfd.FileName);
    }

    private FileService? _fileService;
}
