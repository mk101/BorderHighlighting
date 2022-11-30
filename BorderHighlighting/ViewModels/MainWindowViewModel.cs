using System;
using System.Windows;
using System.Windows.Media;
using BorderHighlighting.Common;
using BorderHighlighting.Common.MVVM;
using OpenCV;

namespace BorderHighlighting.ViewModels;

public class MainWindowViewModel : NotifyPropertyChanged
{
    public MainWindowViewModel()
    {
        var fm = new FileManager();
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

            _ourBitmap = new Bitmap(image, encoder!);
            _baseBitmap = new Bitmap(image, encoder!);

            var img = _cv.GenerateImage();
            _cvBitmap = new Bitmap(img.Pixels, img.Width, img.Height);
            CvImage = _cvBitmap.GetBitmapSource();
        });

        SaveCommand = new RelayCommand(() => { });
    }
    
    public RelayCommand OpenCommand { get; }
    public RelayCommand SaveCommand { get; }

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
