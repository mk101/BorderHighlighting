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
        var hough = new Hough();

        _cv = new OpenCv();

        OpenCommand = new RelayCommand(() =>
        {
            var image = fm.Open();
            var fs = fm.FileService;

            if (image is null)
            {
                return;
            }

            OurImage = image;
            BaseImage = image;
            CvImage = image;

            _ourBitmap = new Bitmap(image, fs!);
            _baseBitmap = new Bitmap(image, fs!);
            _cvBitmap = new Bitmap(image, fs!);
        });

        SaveCommand = new RelayCommand(() => { });

        SobelCommand = new RelayCommand(() =>
        {
            var sourceImage = _baseBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var imageWithGradient = CobelService.Processing(ConvertService.ToGrayscale(sourceImage));
            var resImage = imageWithGradient.Image.GetBitmapSource();
            OurImage = resImage;
        });
        
        PrewittCommand = new RelayCommand(() =>
        {
            var sourceImage = _baseBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var image = PrewittService.Processing(ConvertService.ToGrayscale(sourceImage));
            var resImage = image.GetBitmapSource();
            OurImage = resImage;
        });

        
        CannyCommand = new RelayCommand(() =>
        {
            var sourceImage = _baseBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var image = CannyService.Processing(ConvertService.ToGrayscale(sourceImage));
            var resImage = image.GetBitmapSource();
            OurImage = resImage;
        });


        CannyCvCommand = new RelayCommand(() =>
        {
            if (_baseBitmap is null)
            {
                return;
            }

            var id = ConvertService.BitmapToImageData(_baseBitmap);
            var img = _cv.Canny(id, 100, 200);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });

        HoughCirclesCvCommand = new RelayCommand(() =>
        {
            if (_baseBitmap is null)
            {
                return;
            }
            var id = ConvertService.BitmapToImageData(_baseBitmap);
            var img = _cv.HoughCircles(id, 30, 80);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });
        
        HoughCirclesCommand = new RelayCommand(() =>
        {
            if (_baseBitmap is null || _ourBitmap is null)
            {
                return;
            }

            var circles = houghCircles.FindCircles(CannyService.Processing(ConvertService.ToGrayscale(_baseBitmap)), 500);

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


         HoughLineCommand = new RelayCommand(() =>
        {
            if (_baseBitmap is null || _ourBitmap is null)
            {
                return;
            }
            var lines = hough.FindLines(CannyService.Processing(ConvertService.ToGrayscale(_baseBitmap)), 350);

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
            if (_baseBitmap is null)
            {
                return;
            }
            
            var id = ConvertService.BitmapToImageData(_baseBitmap);
            var img = _cv.HoughLines(id, 200, 180, 100);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();

        });

        SobelCvCommand = new RelayCommand(() =>
        {
            if (_baseBitmap is null)
            {
                return;
            }
            
            var id = ConvertService.BitmapToImageData(_baseBitmap);
            var img = _cv.Sobel(id);
            _cvBitmap = new Bitmap(img);
            CvImage = _cvBitmap.GetBitmapSource();
        });
    }

    public RelayCommand OpenCommand { get; }
    public RelayCommand SaveCommand { get; }

    public RelayCommand HoughCirclesCvCommand { get; }
    public RelayCommand HoughCirclesCommand { get; }

    public RelayCommand SobelCommand { get; }
    public RelayCommand SobelCvCommand { get; }
    public RelayCommand PrewittCommand { get; }

    public RelayCommand CannyCommand { get; }
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
