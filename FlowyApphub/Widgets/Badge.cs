using Gtk;

namespace FlowyApphub.Widgets;

public class Badge : Box
{
    public Badge(string iconName, string cssClass = "success") : this(Image.NewFromIconName(iconName), cssClass)
    {
        
    }
    
    public Badge(Image badgeImage, string cssClass = "success")
    {
        badgeImage.SetSizeRequest(40, 40);
        
        var cssProvider = CssProvider.New();
        cssProvider.LoadFromString(@"
            .badge-success {
                background-color: #007c3d64;
                color: #8ff0a4;
            }

            .badge-warning {
                background-color: #90540064;
                color: #f9f06b;
            }

            .badge-error {
                background-color: #c3000064;
                color: #ff938c;
            }

            .circle {
                border-radius: 50%;
            }
        ");
        // StyleContext.AddProviderForDisplay(GetDisplay(), cssProvider, 999);
        GetStyleContext().AddProvider(cssProvider, 999);
        // AddCssClass(cssClass);
        AddCssClass($"badge-{cssClass}");
        AddCssClass("circle");
        Append(badgeImage);
    }
}