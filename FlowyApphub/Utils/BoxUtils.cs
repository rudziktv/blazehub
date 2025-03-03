using Gtk;

namespace FlowyApphub.Utils;

public static class BoxUtils
{
    public static Box Spacer(Orientation orientation = Orientation.Horizontal, int spacing = 0)
    {
        var box = Box.New(orientation, spacing);
        if (orientation == Orientation.Horizontal)
            box.SetHexpand(true);
        else
            box.SetVexpand(true);
        return box;
    }
    
    public static void Append(this Box box, params Widget[] children)
    {
        foreach (var widget in children)
        {
            box.Append(widget);
        }
    }
    
    public static void Clear(this Box box)
    {
        while (box.GetLastChild() is { } child)
        { 
            box.Remove(child);
        }
    }
}