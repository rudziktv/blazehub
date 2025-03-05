using Adw;
using BlazeHub.AdwGtkFramework.List;
using BlazeHub.Services.Flatpak;
using BlazeHub.Utils;
using BlazeHub.Widgets;
using Gtk;

namespace BlazeHub.Views;

public class UpdateView : Box
{
    private readonly Box _contentBox;
    // private readonly ListBox _updateList;
    private readonly CustomListBox<FlatpakAction, UpdateItemWidget> _updateCustomList;

    public UpdateView()
    {
        // _updateList = ListBox.New();
        // _updateList.AddCssClass("boxed-list");
        // _updateList.SetSelectionMode(SelectionMode.None);

        _updateCustomList =
            new CustomListBox<FlatpakAction, UpdateItemWidget>(FlatpakQueue.Queue,
                i => new UpdateItemWidget(i),
                a => a.AppTarget);
        _updateCustomList.AddCssClass("boxed-list");
        _updateCustomList.SetSelectionMode(SelectionMode.None);
        
        _contentBox = Box.New(Orientation.Vertical, 0);
        _contentBox.SetMargins(12);
        // _contentBox.Append(_updateList);
        _contentBox.Append(_updateCustomList);

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
        
        // FlatpakQueue.OnQueueChanged += Update;
    }

    // private void Update()
    // {
    //     _updateList.Clear();
    //
    //     foreach (var action in FlatpakQueue.Queue)
    //     {
    //         var item = new UpdateItemWidget(action);
    //         _updateList.Append(item);
    //     }
    // }
}