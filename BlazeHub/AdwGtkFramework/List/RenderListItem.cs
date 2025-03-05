using Gtk;

namespace BlazeHub.AdwGtkFramework.List;

public class RenderListItem<T>(string key, T widget) where T : Widget
{
    public string Key { get; set; } = key;
    public T Widget => widget;
}