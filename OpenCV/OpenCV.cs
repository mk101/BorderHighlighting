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
