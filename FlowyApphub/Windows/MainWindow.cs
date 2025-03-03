using System.Diagnostics.CodeAnalysis;
using Adw;
using FlowyApphub.Utils;
using FlowyApphub.Views;
using Gio;
using GObject;
using Gtk;
using Box = Gtk.Box;
using Button = Gtk.Button;
using HeaderBar = Adw.HeaderBar;
using SearchEntry = Gtk.SearchEntry;
using Window = Gtk.Window;

namespace FlowyApphub.Windows;

public class MainWindow : Adw.ApplicationWindow
{
    public static MainWindow Instance { get; set; } = null!;
    public static NavigationView Navigation { get; set; } = null!;
    // public static ViewSwitcher Type { get; set; }

    public MainWindow()
    {
        Instance = this;
        Title = "Flowy Apphub";
        SetDefaultSize(900, 600);

        var viewSwitcher = Adw.ViewSwitcher.New();
        viewSwitcher.SetPolicy(ViewSwitcherPolicy.Wide);        

        var viewStack = Adw.ViewStack.New();
        viewStack.AddTitledWithIcon(new SearchView(), "view_stack_search", "Search", "folder-saved-search-symbolic");
        viewStack.AddTitledWithIcon(new InstalledAppsView(), "view_stack_installed", "Installed", "view-grid-symbolic");
        viewStack.AddTitledWithIcon(new UpdateView(), "view_stack_updates", "Updates", "update-symbolic");
        viewSwitcher.SetStack(viewStack);

        var more = Button.New();
        var moreCon = ButtonContent.New();
        moreCon.SetIconName("open-menu-symbolic");
        more.SetChild(moreCon);
        
        more.OnClicked += MoreButtonClicked;
        

        var header = HeaderBar.New();
        header.PackEnd(more);
        header.SetShowEndTitleButtons(true);
        header.SetTitleWidget(viewSwitcher);
        header.SetShowBackButton(true);

        var toolbar = ToolbarView.New();
        toolbar.SetTopBarStyle(ToolbarStyle.Flat);
        toolbar.AddTopBar(header);
        toolbar.SetContent(viewStack);

        var navigationView = NavigationView.New();
        Navigation = navigationView;

        var mainStack = NavigationPage.New(toolbar, "Main Stack");
        navigationView.Add(mainStack);
        
        
        SetContent(navigationView);
    }

    private void MoreButtonClicked(Button sender, EventArgs args)
    {
        var menuModel = new Gio.Menu();
        menuModel.Append("About", "app.about");
        var moreMenu = PopoverMenu.NewFromModel(menuModel);

        var before = sender.Child;
        moreMenu.SetParent(sender);
        moreMenu.Show();
        
        moreMenu.OnClosed += MenuClosed;

        [SuppressMessage("ReSharper", "AccessToDisposedClosure")]
        void MenuClosed(Popover popover, EventArgs eventArgs)
        {
            moreMenu.Hide();
            sender.SetChild(before);
            moreMenu.OnClosed -= MenuClosed;
            moreMenu.Dispose();
        }
    }
}