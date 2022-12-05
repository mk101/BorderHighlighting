using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace OpenCV;

public class OpenCv
{
    public ImageData Canny(ImageData source, double thresh, double threshLinking)
    {
        var img = new Image<Bgra, byte>(source.Width, source.Height);
        img.Bytes = source.Pixels;
        var canny = img.Canny(thresh, threshLinking);

        return new ImageData(canny.Bytes, canny.Width, canny.Height, ImageData.ChannelsType.Gray);
    }
    
    public ImageData Sobel(ImageData source)
    {
        var img = new Image<Bgra, byte>(source.Width, source.Height);
        img.Bytes = source.Pixels;
        var gray = img.Convert<Gray, byte>();
        var sobel = gray.Sobel(1, 0, 3).AbsDiff(new Gray(0.0));
        var res = sobel.Convert<Gray, byte>();
        return new ImageData(res.Bytes, res.Width, res.Height, ImageData.ChannelsType.Gray);
    }


    public ImageData HoughCircles(ImageData source, double cannyTresh, double accumThresh)
    {
        var img = new Image<Bgra, byte>(source.Width, source.Height);
        img.Bytes = source.Pixels;
        
        var circles = img.HoughCircles(new Bgra(cannyTresh, cannyTresh, cannyTresh, 255), new Bgra(accumThresh, accumThresh, accumThresh, 255), 1, 35, 100, 200);

        for (int i = 0; i < circles.Length; i++)
        {
            for (int j = 0; j < circles[i].Length; j++)
            {
                img.Draw(circles[i][j], new Bgra(.0, .0, 255.0, 255.0), 2);
            }
        }

        return new ImageData(img.Bytes, img.Width, img.Height, ImageData.ChannelsType.Brga);
     }

    public ImageData HoughLines(ImageData source, double cannyThresh, double cannyThreshLinking, int threshold)
    {
        var img = new Image<Bgra, byte>(source.Width, source.Height);
        img.Bytes = source.Pixels;

        var lines = img.HoughLines(cannyThresh, cannyThreshLinking, 1.0, Math.PI/180, threshold, 1, 1.0);

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[i].Length; j++)
            {
                img.Draw(lines[i][j], new Bgra(.0,.0,255.0,255.0), 1);

            }
        }

        return new ImageData(img.Bytes, img.Width, img.Height, ImageData.ChannelsType.Brga);
    }

    public ImageData GenerateHelloWorldImage()
    {
        Mat mat = new Mat(200, 400, DepthType.Cv8U, 4);
        mat.SetTo(new Bgra(255, 0, 0, 255).MCvScalar);

        CvInvoke.PutText(
            mat,
            "Hello world",
            new System.Drawing.Point(10, 80),
            FontFace.HersheyComplex,
            1.0,
            new Bgra(0, 255, 0, 255).MCvScalar
        );


        var img = mat.ToImage<Bgra, byte>();
        // byte[] array = new byte[4 * 200 * 400];
        // int index = 0;
        // for (int i = 0; i < mat.Rows; i++)
        // {
        //     for (int j = 0; j < mat.Cols; j++)
        //     {
        //         var bgra = img[i,j];
        //         array[index] = (byte) (bgra.Blue);
        //         array[index + 1] = (byte) (bgra.Green);
        //         array[index + 2] = (byte) (bgra.Red);
        //         array[index + 3] = (byte) (bgra.Alpha);
        //
        //         index += 4;
        //     }
        // }

        return new ImageData(img.Bytes, img.Width, img.Height, ImageData.ChannelsType.Brga);
    }
}
