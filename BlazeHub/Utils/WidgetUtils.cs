using Gtk;

namespace BlazeHub.Utils;

public static class WidgetUtils
{
    public delegate bool RemoveItemCallback<in TSource>(TSource item);
    public delegate Widget MapCallback<in TSource>(TSource item);
    
    public static void Clear(this ListBox widget)
    {
        while (widget.GetLastChild() is { } child)
        { 
            widget.Remove(child);
        }
    }

    public static void IntelligentUpdate<TWidget, TArray>(this ListBox listBox, IEnumerable<TArray> list,
        MapCallback<TArray> mapCallback, RemoveItemCallback<TWidget> removeWidgetCallback) where TWidget : Widget
    {
        
    }
}