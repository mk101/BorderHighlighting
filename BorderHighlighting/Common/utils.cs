namespace BorderHighlighting.Common;

public class Utils
{
    public static byte Clamp(double value, byte min = 0, byte max = 255)
    {
        if (value < min)
        {
            return min;
        } else if (value > max)
        {
            return max;
        }

        return (byte)value;
    }
}