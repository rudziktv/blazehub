using Adw;
using BlazeHub.Models.Flatpak;
using BlazeHub.Services.Flatpak;
using BlazeHub.Utils;
using BlazeHub.Widgets;
using BlazeHub.Windows;
using Gtk;
using Spinner = Adw.Spinner;

namespace BlazeHub.Views;

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
        
        if (FlatpakService.IsRefreshing)
            SetSpinner();

        GLib.Functions.IdleAdd(1, () =>
        {
            SetAppsList(FlatpakService.InstalledFlatpakApps);
            return false;
        });
        FlatpakService.OnFlatpakChangeReceived += SetSpinner;
        FlatpakService.OnInstalledAppsChanged += OnInstalledAppsChanged;
    }

    private void OnInstalledAppsChanged()
    {
        GLib.Functions.IdleAdd(0, () =>
        {
            SetAppsList(FlatpakService.InstalledFlatpakApps);
            return false;
        });
    }

    private void SetSpinner()
    {
        GLib.Functions.IdleAdd(0, () =>
        {
            _contentBox.Clear();
            _contentBox.Append(Spinner.New());
            return false;
        });
    }

    private void SetAppsList(IEnumerable<InstalledFlatpakApp> apps)
    {
        _appsList.Clear();

        foreach (var app in apps)
        {
            _appsList.Append(new InstalledAppWidget(app).ToBox());
        }
        
        _contentBox.Clear();
        _contentBox.Append(_appsList);
    }
}