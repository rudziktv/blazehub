using Adw;
using FlowyApphub.Services.Flatpak;
using FlowyApphub.Utils;
using FlowyApphub.Widgets;
using Gtk;

namespace FlowyApphub.Views;

public class UpdateView : Box
{
    private Box _contentBox;
    private ListBox _updateList;

    public UpdateView()
    {
        _updateList = ListBox.New();
        _updateList.AddCssClass("boxed-list");
        _updateList.SetSelectionMode(SelectionMode.None);
        
        _contentBox = Box.New(Orientation.Vertical, 0);
        _contentBox.SetMargins(12);
        _contentBox.Append(_updateList);

        var clamp = Clamp.New();
        clamp.SetChild(_contentBox);
        clamp.SetOrientation(Orientation.Horizontal);
        clamp.SetMaximumSize(768);

        var scrollView = ScrolledWindow.New();
        scrollView.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        scrollView.SetHexpand(true);
        scrollView.SetVexpand(true);
        scrollView.SetChild(clamp);
        Append(scrollView);
        
        FlatpakQueue.OnQueueChanged += Update;
    }

    private void Update()
    {
        _updateList.Clear();

        foreach (var action in FlatpakQueue.Queue)
        {
            var item = new UpdateItemWidget(action);
            _updateList.Append(item);
        }
    }
}