using GdkPixbuf;
using Gtk;

namespace BlazeHub.Models.Flathub;

public class AppScreenshot(Image imageWidget, Pixbuf pixbuf, string url, int width, int height, string caption = "")
{
    public string Url { get; private set; } = url;
    public Pixbuf Pixbuf { get; private set; } = pixbuf;
    public Image ImageWidget { get; private set; } = imageWidget;
    public string Caption { get; private set; } = caption;

    public int Width { get; private set; } = width;
    public int Height { get; private set; } = height;
}