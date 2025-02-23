using Gtk;

namespace FlowyApphub.Utils;

public static class BoxUtils
{
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