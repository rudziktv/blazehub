using System.Reflection;
using FlowyApphub.Windows;
using Gdk;
using GdkPixbuf;
using Gio;
using GLib;
using GObject;
using Gtk;
using AboutDialog = Adw.AboutDialog;
using AppInfo = FlowyApphub.Services.Data.AppInfo;
using Application = Adw.Application;
using File = System.IO.File;
using Functions = Gio.Functions;

namespace FlowyApphub;

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
        Application.SetVersion("0.1a");
        
        var weirdPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/com.flamedev.flowyapphub.gresource";
        var res = Functions.ResourceLoad(weirdPath);
        Functions.ResourcesRegister(res);

        var theme = Gtk.IconTheme.GetForDisplay(Gdk.Display.GetDefault());
        theme.AddResourcePath("/com/flamedev/flowyapphub/icons/scalable/status/");
        
        if (!File.Exists(weirdPath))
        {
            Console.WriteLine($"GResource file not found at: {weirdPath}");
        }

        
        foreach (var asset in res.EnumerateChildren("/com/flamedev/flowyapphub/icons/scalable/status/", ResourceLookupFlags.None))
        {
            Console.WriteLine(asset);
        }
        
        
        
        var action = SimpleAction.New("about", null);
        action.OnActivate += (_, _) =>
        {
            var about = AboutDialog.New();
            about.SetApplicationName(AppInfo.APP_NAME);
            about.SetDeveloperName("rudzik.tv");
            about.SetLicenseType(License.Gpl30);
            try
            {
                about.SetApplicationIcon("view-grid-symbolic");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            about.SetVersion(Application.Version ?? "error");
            
            about.Present(Application.ActiveWindow);
        };
        Application.AddAction(action);
        
        
        // application.AddAction(Action);
        

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

        // Start the application's event loop and process user interactions.
        // RunWithSynchronizationContext ensures proper thread synchronization for GTK.
        // The "null" parameter takes the arguments from the commandline. As there are no arguments
        // supported in this tutorial the parameter is not filled and thus null.
        Application.RunWithSynchronizationContext(null);
        // return;
    }
    
    private static Gdk.Texture LoadFromResource(string resourceName)
    {
        try
        {
            var data = Assembly.GetExecutingAssembly().ReadResourceAsByteArray(resourceName);
            using var bytes = Bytes.New(data);
            var pixbufLoader = PixbufLoader.New();
            pixbufLoader.WriteBytes(bytes);
            pixbufLoader.Close();

            var pixbuf = pixbufLoader.GetPixbuf() ?? throw new Exception("No pixbuf loaded");
            return Gdk.Texture.NewForPixbuf(pixbuf);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unable to load image resource '{resourceName}': {e.Message}");
            return null;
        }
    }
}