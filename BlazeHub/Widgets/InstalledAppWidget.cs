using Adw;
using BlazeHub.AdwGtkFramework;
using BlazeHub.Models.Flatpak;
using BlazeHub.Services.Flatpak;
using BlazeHub.Utils;
using BlazeHub.Views;
using BlazeHub.Windows;
using Gio;
using Gtk;
using AlertDialog = Adw.AlertDialog;

namespace BlazeHub.Widgets;

public class InstalledAppWidget : AdvancedBox
{
    private InstalledFlatpakApp _app;
    
    public InstalledAppWidget(InstalledFlatpakApp app): base(Orientation.Horizontal)
    {
        _app = app;
        
        var clickCtr = GestureClick.New();
        clickCtr.SetPropagationLimit(PropagationLimit.SameNative);
        clickCtr.SetPropagationPhase(PropagationPhase.Bubble);
        clickCtr.OnStopped += (sender, args) => sender.SetState(EventSequenceState.Denied);
        clickCtr.OnReleased += (sender, args) =>
        {
            
            // MainWindow.Navigation.Push(NavigationPage.New(new AppSiteView(), app.Name));
            Console.WriteLine($"MAIN CLICK {sender.Button} {args.NPress}");
            MainWindow.Navigation.Push(ViewUtils.WrapViewIntoPage(new AppSiteView(app.ID), app.Name));
        };
        clickCtr.SetExclusive(false);
        BoundingBox.AddController(clickCtr);
        
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
        uninstallBtn.OnClicked += (sender, args) => UninstallAppDialog(_app, this);

        var splitMenu = Menu.New();
        var updateItem = MenuItem.New("Update", "update-app");
        // updateItem.SetIcon();
        // splitMenu.Append("Update", "gfdsg");
        splitMenu.AppendItem(updateItem);
        var splitUninstallBtn = SplitButton.New();
        // var splitCtr = GestureClick.New();
        // splitCtr.OnPressed += (sender, args) => Console.WriteLine($"SPLIT CLICK {sender.Button}");
        // splitCtr.SetPropagationPhase(PropagationPhase.Capture);
        // splitCtr.SetPropagationLimit(PropagationLimit.SameNative);
        // splitUninstallBtn.AddController(splitCtr);
        splitUninstallBtn.AddCssClass("destructive-action");
        splitUninstallBtn.SetLabel("Uninstall");
        splitUninstallBtn.SetMenuModel(splitMenu);
        splitUninstallBtn.OnClicked += (sender, args) => UninstallAppDialog(_app, this);
        
        
        var installedSize = Label.New(app.InstalledSize);
        installedSize.AddCssClass("dim-label");
        
        var actions = Box.New(Orientation.Vertical, 4);
        actions.SetValign(Align.Center);
        // actions.Append(uninstallBtn);
        actions.Append(splitUninstallBtn);
        actions.Append(installedSize);
        
        // Final assignments
        Append(icon);
        Append(infoBox);
        Append(spacer);
        Append(actions);
    }

    public static void UninstallAppDialog(InstalledFlatpakApp app, Widget parent)
    {
        var dialog = AlertDialog.New($"Uninstall {app.Name}", $"Are you sure you want to uninstall this app?\nThis action might free up {app.InstalledSize} on your disk.");
        
        dialog.AddResponse("cancel", "Dismiss");
        dialog.AddResponse("uninstall", "Uninstall");
        dialog.SetResponseAppearance("uninstall", ResponseAppearance.Destructive);
        // dialog.SetResponseEnabled("uninstall", false);

        dialog.OnResponse += (sender, args) =>
        {
            Console.WriteLine($"{args.Response} - {app.ID}");
            if (args.Response != "uninstall") return;
            _ = FlatpakService.UninstallApp(app.ID);
        };
        
        dialog.Present(parent);
    }
}