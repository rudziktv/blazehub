using Gtk;

namespace BlazeHub.Utils;

public static class MarginUtils
{
    public static void SetMargins(this Widget widget, int left, int right, int top, int bottom)
    {
        widget.SetMarginStart(left);
        widget.SetMarginEnd(right);
        widget.SetMarginTop(top);
        widget.SetMarginBottom(bottom);
    }
    
    public static void SetMargins(this Widget widget, int horizontal, int vertical)
        => widget.SetMargins(horizontal, horizontal, vertical, vertical);
    
    
    public static void SetMargins(this Widget widget, int margin)
        => widget.SetMargins(margin, margin);
}