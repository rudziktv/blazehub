using Gtk;

namespace FlowyApphub.Utils;

public static class WidgetUtils
{
    public static void Clear(this ListBox widget)
    {
        while (widget.GetLastChild() is { } child)
        { 
            widget.Remove(child);
        }
    }
}