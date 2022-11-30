namespace OpenCV;

public struct ImageData
{
    public readonly byte[] Pixels;
    public readonly int Width;
    public readonly int Height;

    public ImageData(byte[] pixels, int width, int height)
    {
        Width = width;
        Height = height;
        
        Pixels = new byte[4 * width * height];
        Array.Copy(pixels, Pixels, 4 * width * height);
    }
}
