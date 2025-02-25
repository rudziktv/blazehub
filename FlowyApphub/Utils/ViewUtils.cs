using Adw;
using Gtk;
using HeaderBar = Adw.HeaderBar;

namespace FlowyApphub.Utils;

public static class ViewUtils
{
    public static NavigationPage WrapViewIntoPage(Widget widget, string title)
    {
        var toolbar = ToolbarView.New();
        var header = HeaderBar.New();
        
        toolbar.AddTopBar(header);
        toolbar.SetTopBarStyle(ToolbarStyle.Flat);
        toolbar.SetContent(widget);
        
        var navigationPage = NavigationPage.New(toolbar, title);
        
        return navigationPage;
    }
}