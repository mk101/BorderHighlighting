using System;
using System.Windows;
using System.Windows.Media;
using BorderHighlighting.Common;
using BorderHighlighting.Common.MVVM;
using BorderHighlighting.Models;

namespace BorderHighlighting.ViewModels;

public class MainWindowViewModel : NotifyPropertyChanged
{
    public MainWindowViewModel()
    {
        var fm = new FileManager();
        
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

        CobelCommand = new RelayCommand(() =>
        {
            var sourceImage = _ourBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var image = CobelService.Processing(sourceImage);
            var resImage = image.GetBitmapSource();
            OurImage = resImage;
        });
        
        PrewittCommand = new RelayCommand(() =>
        {
            var sourceImage = _ourBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var image = PrewittService.Processing(sourceImage);
            var resImage = image.GetBitmapSource();
            OurImage = resImage;
        });
        
        CannyCommand = new RelayCommand(() =>
        {
            var sourceImage = _ourBitmap;
            if (sourceImage == null)
            {
                return;
            }
            
            var image = CannyService.Processing(sourceImage);
            var resImage = image.GetBitmapSource();
            OurImage = resImage;
        });
    }
    
    public RelayCommand OpenCommand { get; }
    public RelayCommand SaveCommand { get; }
    
    public RelayCommand CobelCommand { get; }
    public RelayCommand PrewittCommand { get; }
    public RelayCommand CannyCommand { get; }

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
}
