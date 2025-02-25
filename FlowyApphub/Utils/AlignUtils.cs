using Gtk;

namespace FlowyApphub.Utils;

public static class AlignUtils
{
    public static void Align(this Widget widget, Align alignment)
        => Align(widget, alignment, alignment);
    
    public static void Align(this Widget widget, Align horizontal, Align vertical)
    {
        widget.SetHalign(horizontal);
        widget.SetValign(vertical);
    }
}