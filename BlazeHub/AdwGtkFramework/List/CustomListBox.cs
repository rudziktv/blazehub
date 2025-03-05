using System.Collections.ObjectModel;
using System.Collections.Specialized;
using BlazeHub.Utils;
using Gtk;

namespace BlazeHub.AdwGtkFramework.List;

public class CustomListBox<TSource, TWidget> : ListBox where TWidget : Widget where TSource : class
{
    public delegate string KeyMap(TSource item);
    public delegate TWidget MapCallback(TSource item);

    public int Priority { get; set; } = 0;
    public ObservableCollection<TSource> Collection { get; }

    private bool _renderQueued;
    private readonly KeyMap _keyMap;
    private readonly MapCallback _map;
    private readonly List<RenderListItem<TWidget>> _renderItems = [];

    public CustomListBox(ObservableCollection<TSource> collection, MapCallback map, KeyMap keyMap)
    {
        _map = map;
        _keyMap = keyMap;
        Collection = collection;
        
        if (collection.Count != 0)
            QueueRender();
        
        Collection.CollectionChanged += OnCollectionChanged;
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        QueueRender();
    }

    private void QueueRender()
    {
        if (_renderQueued)
            return;
        _renderQueued = true;
        GLib.Functions.IdleAdd(Priority, Render);
    }

    private bool Render()
    {
        // org.DolphinEmu.dolphin-emu
        Console.WriteLine($"Rerender. _renderItems.Count: {_renderItems.Count}, Collection.Count: {Collection.Count}");
        
        _renderItems.RemoveAll(renderItem =>
        {
            var shouldBeRemoved = Collection.FirstOrDefault(c => _keyMap(c) == renderItem.Key) == null;
            Console.WriteLine($"{renderItem.Key} should be removed {shouldBeRemoved}");
            if (shouldBeRemoved)
            {
                // Console.WriteLine($"hex: {GetLastChild()}");
                if (renderItem.Widget.Parent != null)
                    base.Remove(renderItem.Widget.Parent);
            }
            return shouldBeRemoved;
        });
        
        for (var i = 0; i < Collection.Count; i++)
        {
            var item = Collection[i];
            var key = _keyMap(item);
            var index = _renderItems.FindIndex(renderItem => renderItem.Key == key);
            Console.WriteLine($"Key: {key}, FindIndex: {index}, i: {i}");
            if (index != -1) continue;
            var listItem = new RenderListItem<TWidget>(key, _map(item));
            _renderItems.Add(listItem);
            Insert(listItem.Widget, i);
        }
        
        _renderQueued = false;
        return false;
    }
}