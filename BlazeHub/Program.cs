using System.Reflection;
using BlazeHub.Services;
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
        Application.SetVersion("0.0.0a.dev.preview.1");
        
        var mainResPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/com.flamedev.flowyapphub.gresource";
        var resPath = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "Resources") + "/com.flamedev.flowyapphub.gresource";
        var weirdPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/com.flamedev.flowyapphub.gresource";
        var res = Functions.ResourceLoad(File.Exists(mainResPath) ? mainResPath : resPath);
        Functions.ResourcesRegister(res);

        // Debug resource contents
        try 
        {
            var children = res.EnumerateChildren("/com/flamedev/flowyapphub/icons/scalable/status", ResourceLookupFlags.None);
            Console.WriteLine("Resource contents:");
            foreach (var child in children)
            {
                Console.WriteLine($"Found resource: {child}");
            }
        } 
        catch (Exception e)
        {
            Console.WriteLine($"Error enumerating resources: {e.Message}");
        }

        var theme = Gtk.IconTheme.GetForDisplay(Gdk.Display.GetDefault());
        theme.AddResourcePath("/com/flamedev/flowyapphub/icons");
        theme.AddResourcePath("/var/lib/flatpak/exports/share/icons");
        theme.AddSearchPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local/share/flatpak/exports/share/icons"));
        theme.AddSearchPath("icons");
        theme.AddSearchPath("/var/lib/flatpak/exports/share/icons");
        
        if (!File.Exists(weirdPath))
        {
            Console.WriteLine($"GResource file not found at: {weirdPath}");
        }

        // Debug theme paths
        Console.WriteLine("\nIcon theme search paths:");
        foreach (var path in theme.SearchPath)
        {
            Console.WriteLine(path);
        }

        // Try both with and without the full path
        Console.WriteLine("\nIcon availability:");
        Console.WriteLine($"update-symbolic: {theme.HasIcon("update-symbolic")}");
        Console.WriteLine($"blaze-apphub: {theme.HasIcon("blaze-apphub")}");
        Console.WriteLine($"Full path: {theme.HasIcon("/com/flamedev/flowyapphub/icons/scalable/status/update-symbolic")}");
        
        
        
        var action = SimpleAction.New("about", null);
        action.OnActivate += (_, _) =>
        {
            var about = AboutDialog.New();
            about.SetApplicationName(AppInfo.APP_NAME);
            about.SetDeveloperName("rudzik.tv");
            about.SetLicenseType(License.Gpl30);
            about.SetApplicationIcon("blaze-apphub");
            about.SetVersion(Application.Version ?? "error");
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