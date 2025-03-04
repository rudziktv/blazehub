using Gtk;

namespace BlazeHub.Utils;

public static class ImageUtils
{
    public static void SetImageHeight(this Image image, int desiredHeight, int width, int height)
    {
        var factor = desiredHeight / (float)height;
        var newWidth = (int)MathF.Round(factor * width);
        image.SetSizeRequest(newWidth, desiredHeight);
    }
    
    public static void SetImageHeight(this Image image, int desiredHeight)
    {
        image.SetSizeRequest(200, desiredHeight);
    }
}