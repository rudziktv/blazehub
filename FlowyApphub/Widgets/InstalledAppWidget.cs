using Adw;
using FlowyApphub.Models.Flatpak;
using FlowyApphub.Services.Flatpak;
using FlowyApphub.Utils;
using FlowyApphub.Views;
using FlowyApphub.Windows;
using Gtk;
using AlertDialog = Adw.AlertDialog;

namespace FlowyApphub.Widgets;

public class InstalledAppWidget : Box
{
    private InstalledFlatpakApp _app;
    
    public InstalledAppWidget(InstalledFlatpakApp app)
    {
        _app = app;

        var clickCtr = GestureClick.New();
        clickCtr.OnPressed += (sender, args) =>
        {
            // MainWindow.Navigation.Push(NavigationPage.New(new AppSiteView(), app.Name));
            
            MainWindow.Navigation.Push(ViewUtils.WrapViewIntoPage(new AppSiteView(app.ID), app.Name));
        };
        AddController(clickCtr);
        
        SetOrientation(Orientation.Horizontal);
        this.SetMargins(12);
        SetSpacing(12);
        
        
        var icon = Image.NewFromIconName(app.ID);
        icon.SetIconSize(IconSize.Large);
        icon.PixelSize = 64;
        icon.SetSizeRequest(64, 64);

        var infoBox = Box.New(Orientation.Vertical, 8);
        var name = Label.New(app.Name);
        name.AddCssClass("heading");
        name.SetHalign(Align.Start);
        var version = Label.New(app.Version);
        version.AddCssClass("dim-label");
        version.SetHalign(Align.Start);
        infoBox.Append(name);
        infoBox.Append(version);
        infoBox.SetValign(Align.Center);

        var spacer = Box.New(Orientation.Horizontal, 0);
        spacer.SetHexpand(true);
        
        // ACTIONS
        var uninstallBtn = Button.NewWithLabel("Uninstall");
        uninstallBtn.AddCssClass("destructive-action");
        uninstallBtn.OnClicked += (sender, args) => UninstallAppDialog();
        
        var installedSize = Label.New(app.InstalledSize);
        installedSize.AddCssClass("dim-label");
        
        var actions = Box.New(Orientation.Vertical, 4);
        actions.SetValign(Align.Center);
        actions.Append(uninstallBtn);
        actions.Append(installedSize);
        
        // Final assignments
        Append(icon);
        Append(infoBox);
        Append(spacer);
        Append(actions);
    }

    private void UninstallAppDialog()
    {
        var dialog = AlertDialog.New($"Uninstall {_app.Name}", $"Are you sure you want to uninstall this app?\nThis action might free up {_app.InstalledSize} on your disk.");
        
        dialog.AddResponse("cancel", "Dismiss");
        dialog.AddResponse("uninstall", "Uninstall");
        dialog.SetResponseAppearance("uninstall", ResponseAppearance.Destructive);
        // dialog.SetResponseEnabled("uninstall", false);

        dialog.OnResponse += (sender, args) =>
        {
            Console.WriteLine($"{args.Response} - {_app.ID}");
            if (args.Response != "uninstall") return;
            _ = FlatpakService.UninstallApp(_app.ID);
        };
        
        dialog.Present(this);
    }
}