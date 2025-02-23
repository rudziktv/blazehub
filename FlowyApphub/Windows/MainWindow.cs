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

public class MainWindow : Gtk.ApplicationWindow
{
    public MainWindow()
    {
        AddCssClass("devel");
        Title = "Flowy Apphub";
        SetDefaultSize(900, 600);

        var viewSwitcher = Adw.ViewSwitcher.New();
        viewSwitcher.SetPolicy(ViewSwitcherPolicy.Wide);
        

        var main = Box.New(Orientation.Vertical, 0);
        main.SetMargins(10);
        var ent = SearchEntry.New();
        ent.PlaceholderText = "Search in Flathub...";
        var clamp = Clamp.New();
        clamp.SetOrientation(Orientation.Horizontal);
        clamp.MaximumSize = 450;
        clamp.Child = ent;
        
        main.Append(clamp);
        
        
        

        var viewStack = Adw.ViewStack.New();
        viewStack.AddTitledWithIcon(new AppSiteView(), "view_stack_search", "Search", "folder-saved-search-symbolic");
        viewStack.AddTitledWithIcon(Box.New(Orientation.Vertical, 0), "view_stack_installed", "Installed", "view-grid-symbolic");
        viewStack.AddTitledWithIcon(Box.New(Orientation.Vertical, 0), "view_stack_updates", "Updates", "software-update-available-symbolic");
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
        SetTitlebar(header);
        
        Child = viewStack;
    }

    private void MoreButtonClicked(Button sender, EventArgs args)
    {
        var wtf = new Gio.Menu();
        wtf.Append("About", "app.about");
        var moreMenu = PopoverMenu.NewFromModel(wtf);

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