using Adw;
using FlowyApphub.Models.Flatpak;
using FlowyApphub.Services.Dialog;
using FlowyApphub.Services.Flatpak;
using FlowyApphub.Utils;
using FlowyApphub.Widgets;
using Gtk;

namespace FlowyApphub.Views;

public class InstalledAppsView : Box
{
    private ListBox _appsList;
    
    public InstalledAppsView()
    {
        SetOrientation(Orientation.Vertical);
        var scrollView = ScrolledWindow.New();
        scrollView.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        scrollView.SetVexpand(true);

        var contentBox = Box.New(Orientation.Vertical, 0);
        contentBox.SetMargins(12);
        contentBox.Append(Label.New("This is installed app page-view"));

        _appsList = ListBox.New();
        _appsList.AddCssClass("boxed-list");
        _appsList.SetSelectionMode(SelectionMode.None);
        
        contentBox.Append(_appsList);

        var clampBox = Clamp.New();
        clampBox.SetChild(contentBox);
        clampBox.SetOrientation(Orientation.Horizontal);
        clampBox.SetMaximumSize(600);
        
        scrollView.SetChild(clampBox);
        Append(scrollView);
        
        RefreshAppsList();
    }

    private async void RefreshAppsList()
    {
        try
        {
            SetAppsList(await FlatpakService.GetAppsList());
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }

    private void SetAppsList(List<InstalledFlatpakApp> apps)
    {
        _appsList.Clear();

        foreach (var app in apps)
        {
            // var appBox = Box.New(Orientation.Vertical, 4);
            // appBox.SetMargins(12);
            // appBox.Append(Label.New(app.Name));
            // appBox.Append(Label.New(app.Version));
            //
            // appBox.Append(Image.NewFromIconName(app.ID));
            
            _appsList.Append(new InstalledAppWidget(app));
        }
    }
}