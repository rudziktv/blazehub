using Adw;
using FlowyApphub.Models.Flatpak;
using FlowyApphub.Services.Dialog;
using FlowyApphub.Services.Flatpak;
using FlowyApphub.Utils;
using FlowyApphub.Widgets;
using FlowyApphub.Windows;
using Gtk;
using Spinner = Adw.Spinner;

namespace FlowyApphub.Views;

public class InstalledAppsView : Box
{
    private readonly Box _contentBox;
    private ListBox _appsList;
    
    public InstalledAppsView()
    {
        var banner = Banner.New("Work in progress!");
        banner.SetRevealed(true);
        banner.SetButtonLabel("Go to Test View");
        banner.OnButtonClicked += (sender, args) =>
        {
            MainWindow.Navigation.Push(ViewUtils.WrapViewIntoPage(new AppSiteView("org.gnome.World.Iotas"), "Test View"));
            // Console.WriteLine("Button clicked");
        };
        
        SetOrientation(Orientation.Vertical);
        var scrollView = ScrolledWindow.New();
        scrollView.SetPolicy(PolicyType.Never, PolicyType.Automatic);
        scrollView.SetVexpand(true);

        _contentBox = Box.New(Orientation.Vertical, 0);
        _contentBox.SetMargins(12);
        _contentBox.Append(Label.New("This is installed app page-view"));

        _appsList = ListBox.New();
        _appsList.AddCssClass("boxed-list");
        _appsList.SetSelectionMode(SelectionMode.None);
        
        _contentBox.Append(_appsList);

        var clampBox = Clamp.New();
        clampBox.SetChild(_contentBox);
        clampBox.SetOrientation(Orientation.Horizontal);
        clampBox.SetMaximumSize(600);
        
        scrollView.SetChild(clampBox);
        Append(banner);
        Append(scrollView);
        
        // RefreshAppsList();
        
        if (FlatpakService.IsRefreshing)
            SetSpinner();

        FlatpakService.OnFlatpakChangeReceived += SetSpinner;
        FlatpakService.OnInstalledAppsChanged += () => SetAppsList(FlatpakService.InstalledFlatpakApps);
        
        FlatpakService.RefreshFlatpakAppList();

        // FlatpakListener.OnFlatpakFolderChanged += async() =>
        // {
        //     // _appsList.Clear();
        //     // Console.WriteLine("OnFlatpakChanged");
        //     // SetSpinner();
        //     // await Task.Delay(2000);
        //     // RefreshAppsList();
        // };
    }

    // private async void RefreshAppsList()
    // {
    //     try
    //     {
    //         SetSpinner();
    //         SetAppsList(FlatpakService.InstalledFlatpakApps);
    //     }
    //     catch (Exception e)
    //     {
    //         ErrorDialogService.ShowErrorDialog(e);
    //         Console.WriteLine(e);
    //     }
    // }

    private void SetSpinner()
    {
        _contentBox.Clear();
        _contentBox.Append(Spinner.New());
    }

    private void SetAppsList(IEnumerable<InstalledFlatpakApp> apps)
    {
        _appsList.Clear();

        foreach (var app in apps)
        {
            _appsList.Append(new InstalledAppWidget(app));
        }
        
        _contentBox.Clear();
        _contentBox.Append(_appsList);
    }
}