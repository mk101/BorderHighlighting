namespace OpenCV;

public struct ImageData
{
    public readonly byte[] Pixels;
    public readonly int Width;
    public readonly int Height;
    public readonly ChannelsType Channels;

    public ImageData(byte[] pixels, int width, int height, ChannelsType channels)
    {
        Width = width;
        Height = height;
        Channels = channels;

        var ch = channels switch
        {
            ChannelsType.Gray => 1,
            ChannelsType.Brga => 4,
            _ => 4
        };

        Pixels = new byte[ch * width * height];
        Array.Copy(pixels, Pixels, ch * width * height);
    }
    
    public enum ChannelsType
    {
        Gray,
        Brga
    }
}
