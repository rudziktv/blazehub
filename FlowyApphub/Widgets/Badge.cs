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
                background-color: #007c3d72;
            }

            .badge-warning {
                background-color: #90540072;
            }

            .badge-error {
                background-color: #c3000072;
            }

            .circle {
                border-radius: 50%;
            }
        ");
        // StyleContext.AddProviderForDisplay(GetDisplay(), cssProvider, 999);
        GetStyleContext().AddProvider(cssProvider, 999);
        // AddCssClass(cssClass);
        AddCssClass($"badge-{cssClass}");
        badgeImage.AddCssClass(cssClass);
        AddCssClass("circle");
        Append(badgeImage);
    }
}