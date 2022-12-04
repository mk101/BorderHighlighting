using System;
using System.Windows;
using System.Windows.Media;
using BorderHighlighting.Common;
using BorderHighlighting.Common.MVVM;
using BorderHighlighting.Models;
using OpenCV;
using Color = BorderHighlighting.Common.Color;

namespace BorderHighlighting.ViewModels;

public class MainWindowViewModel : NotifyPropertyChanged
{
    public MainWindowViewModel()
    {
        var fm = new FileManager();
        var hough = new Hough();
        _cv = new OpenCv();
        
        OpenCommand = new RelayCommand(() =>
        {
            var image = fm.Open();
            var encoder = fm.Encoder;

            if (image is null)
            {
                return;
            }
            
            OurImage = image;
            BaseImage = image;
            CvImage = image;

            _ourBitmap = new Bitmap(image, encoder!);
            _baseBitmap = new Bitmap(image, encoder!);
            _cvBitmap = new Bitmap(image, encoder!);
        });

        SaveCommand = new RelayCommand(() => { });

        CannyCvCommand = new RelayCommand(() =>
        {
            if (_cvBitmap is null)
            {
                return;
            }
            
            var id = ConvertService.BitmapToImageData(_cvBitmap);
            var img = _cv.Canny(id, 100, 200);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });

        HoughLineCommand = new RelayCommand(() =>
        {
            if (_ourBitmap is null)
            {
                return;
            }

            var lines = hough.FindLines(_ourBitmap);

            foreach (var line in lines)
            {
                if (line.K is null)
                {
                    if (line.X < 0 || line.X >= _ourBitmap.Width)
                    {
                        continue;
                    }
                    for (int i = 0; i < _ourBitmap.Height; i++)
                    {
                        _ourBitmap.SetColor((int) line.X, i, new Color(255, 0, 0));
                    }
                    
                    continue;
                }

                for (int i = 0; i < _ourBitmap.Width; i++)
                {
                    int y = (int) ((line.K ?? 0) * i + line.B);
                    if (y < 0 || y >= _ourBitmap.Height)
                    {
                        continue;
                    }
                    _ourBitmap.SetColor(i, y, new Color(255, 0, 0));
                }
            }

            OurImage = _ourBitmap.GetBitmapSource();
        });

        HoughLineCvCommand = new RelayCommand(() =>
        {
            if (_cvBitmap is null)
            {
                return;
            }
            
            var id = ConvertService.BitmapToImageData(_cvBitmap);
            var img = _cv.HoughLines(id, 200, 180, 100);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });
    }
    
    public RelayCommand OpenCommand { get; }
    public RelayCommand SaveCommand { get; }
    
    public RelayCommand CannyCvCommand { get; }
    
    public RelayCommand HoughLineCommand { get; }
    public RelayCommand HoughLineCvCommand { get; }

    public ImageSource? OurImage
    {
        get => _ourImage;
        set
        {
            if (Equals(value, _ourImage))
            {
                return;
            }

            _ourImage = value;
            OnPropertyChanged();
        }
    }

    public ImageSource? BaseImage
    {
        get => _baseImage;
        set
        {
            if (Equals(value, _baseImage))
            {
                return;
            }

            _baseImage = value;
            OnPropertyChanged();
        }
    }

    public ImageSource? CvImage
    {
        get => _cvImage;
        set
        {
            if (Equals(value, _cvImage))
            {
                return;
            }

            _cvImage = value;
            OnPropertyChanged();
        }
    }

    private ImageSource? _ourImage;
    private ImageSource? _baseImage;
    private ImageSource? _cvImage;

    private Bitmap? _ourBitmap;
    private Bitmap? _baseBitmap;
    private Bitmap? _cvBitmap;

    private OpenCv _cv;
}
