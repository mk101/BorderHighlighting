using System;
using System.Windows;
using System.Windows.Input;
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
        var houghCircles = new HoughCircle();
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

        HoughCirclesCvCommand = new RelayCommand(() =>
        {
            if (_cvBitmap is null)
            {
                return;
            }

            var id = ConvertService.BitmapToImageData(_cvBitmap);
            var img = _cv.HoughCircles(id, 30, 80);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });

        HoughCirclesCommand = new RelayCommand(() =>
        {
            if (_ourBitmap is null)
            {
                return;
            }
            int minR = 0;
            var circles = houghCircles.FindCircles(_ourBitmap, 500);

            foreach (var circle in circles)
            {
                for (int i = 0; i < 361; i++)
                {
                    double rad = (Math.PI / 180) * i;
                    var cos = Math.Cos(rad);
                    var sin = Math.Sin(rad);
                    int x = (int)(circle.X0 + circle.R * cos);
                    int y = (int)(circle.Y0 + circle.R * sin);
                    if (y < 0 || y >= _ourBitmap.Height || x < 0 || x >= _ourBitmap.Width)
                    {
                        continue;
                    }
                    _ourBitmap.SetColor(x, y, new Color(255, 0, 0));
                }
            }

            OurImage = _ourBitmap.GetBitmapSource();
        });
    }

    public RelayCommand OpenCommand { get; }
    public RelayCommand SaveCommand { get; }

    public RelayCommand CannyCvCommand { get; }
    public RelayCommand HoughCirclesCvCommand { get; }
    public RelayCommand HoughCirclesCommand { get; }

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
