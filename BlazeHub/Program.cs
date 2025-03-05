using System.Reflection;
using BlazeHub.Services;
using BlazeHub.Services.AppResources;
using BlazeHub.Windows;
using Gio;
using Gtk;
using AboutDialog = Adw.AboutDialog;
using AppInfo = BlazeHub.Services.Data.AppInfo;
using Application = Adw.Application;
using File = System.IO.File;
using Functions = Gio.Functions;

namespace BlazeHub;

class Program
{
    public static Application Application { get; private set; }
    
    static void Main(string[] args)
    {
        // Create a new GTK application instance.
        // "com.example.helloworld" is the unique application ID used to identify the app.
        // The application ID should be a domain name you control. 
        // If you don't own a domain name you can use a project specific domain such as github pages. 
        // e.g. io.github.projectname
        // Gio.ApplicationFlags.FlagsNone indicates no special flags are being used.
        Application = Adw.Application.New(AppInfo.APP_ID, Gio.ApplicationFlags.FlagsNone); 
        Application.SetVersion(AppInfo.APP_VERSION);
        
        AppResources.InitializeAppResources();
        
        
        
        var action = SimpleAction.New("about", null);
        action.OnActivate += (_, _) =>
        {
            var about = AboutDialog.New();
            about.SetApplicationName(AppInfo.APP_NAME);
            about.SetDeveloperName("rudzik.tv");
            about.SetLicenseType(License.Gpl30);
            about.SetApplicationIcon("blaze-apphub");
            about.SetVersion(Application.Version ?? "error");
            about.SetWebsite("https://github.com/rudziktv/blazehub");
            about.SetIssueUrl("https://github.com/rudziktv/blazehub/issues/new/choose");
            about.Present(Application.ActiveWindow);
        };
        Application.AddAction(action);

        // Attach an event handler to the application's "OnActivate" event.
        // This event is triggered when the application is started or activated.
        Application.OnActivate += (sender, args) =>
        {
            // Create a new instance of the main application window.
            var window = new MainWindow();
            // Set the "Application" property of the window to the current application instance.
            // This links the window to the application, allowing them to work together.
            window.Application = (Adw.Application) sender;
            
            // Show the window on the screen.
            // This makes the window visible to the user.
            window.Show();
        };
        AppServices.StartAppServices();

        // Start the application's event loop and process user interactions.
        // RunWithSynchronizationContext ensures proper thread synchronization for GTK.
        // The "null" parameter takes the arguments from the commandline. As there are no arguments
        // supported in this tutorial the parameter is not filled and thus null.
        Application.RunWithSynchronizationContext(null);
    }
}