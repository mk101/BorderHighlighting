using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace OpenCV;

public class OpenCv
{
    public byte[] GenerateImage()
    {
        Mat img = new Mat(200, 400, DepthType.Cv8U, 4);
        img.SetTo(new Bgr(255, 0, 0).MCvScalar);
        
        CvInvoke.PutText(
            img,
            "Hello world",
            new System.Drawing.Point(10, 80),
            FontFace.HersheyComplex,
            1.0,
            new Bgr(0, 255, 0).MCvScalar
        );
        
        return img.GetRawData();
    }
}
