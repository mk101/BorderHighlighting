using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BorderHighlighting.Common;

public class Bitmap
{
    public Bitmap(BitmapSource image, BitmapEncoder encoder)
    {
        _writeableBitmap = new WriteableBitmap(image);
        _data = new BitmapData
        {
            Width = image.PixelWidth,
            Height = image.PixelHeight,
            Stride = 4 * image.PixelWidth,
            Pixels = new byte[4 * image.PixelWidth * image.PixelHeight]
        };
        _encoder = encoder;
        _writeableBitmap.CopyPixels(_data.Pixels, _data.Stride, 0);
    }

    public Bitmap(Bitmap source)
    {
        _writeableBitmap = source._writeableBitmap;
        _encoder = source._encoder;
        _data = new BitmapData
        {
            Width = source.Width,
            Height = source.Height,
            Stride = 4 * source.Width,
            Pixels = new byte[4 * source.Width * source.Height]
        };
        _writeableBitmap.CopyPixels(_data.Pixels, _data.Stride, 0);
    }

    public int Width => _data.Width;
    public int Height => _data.Height;

    public Color GetColor(int x, int y)
    {
        if ((x < 0) || (x >= _data.Width))
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }
        
        if ((y < 0) || (y >= _data.Height))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        var index = y * _data.Stride + 4 * x;
        return new Color(
            _data.Pixels[index],
            _data.Pixels[index + 1],
            _data.Pixels[index + 2],
            _data.Pixels[index + 3]
        );
    }

    public void SetColor(int x, int y, Color color)
    {
        if ((x < 0) || (x >= _data.Width))
        {
            throw new ArgumentOutOfRangeException(nameof(x));
        }
        
        if ((y < 0) || (y >= _data.Height))
        {
            throw new ArgumentOutOfRangeException(nameof(y));
        }

        var index = y * _data.Stride + 4 * x;
        var pixels = _data.Pixels;
        
        pixels[index] = color.R;
        pixels[index + 1] = color.G;
        pixels[index + 2] = color.B;
        pixels[index + 3] = color.A;
    }

    public BitmapSource GetBitmapSource()
    {
        _writeableBitmap.WritePixels(new Int32Rect(0, 0, _data.Width, _data.Height), _data.Pixels, _data.Stride, 0);
        return ConvertService.WritableBitmapToBitmapImage(_writeableBitmap, _encoder);
    }

    private struct BitmapData
    {
        public byte[] Pixels;
        public int Width;
        public int Height;
        public int Stride;
    }
    
    private WriteableBitmap _writeableBitmap;
    private readonly BitmapData _data;
    private readonly BitmapEncoder _encoder;
}
