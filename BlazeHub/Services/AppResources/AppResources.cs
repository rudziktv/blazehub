using System.Reflection;
using BlazeHub.Services.Flatpak.Dirs;
using Gio;
using Gtk;
using File = System.IO.File;
using Functions = Gio.Functions;

namespace BlazeHub.Services.AppResources;

public static class AppResources
{
    public static void InitializeAppResources()
    {
        var mainResPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/com.flamedev.flowyapphub.gresource";
        var resPath = Path.Combine(Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "Resources") + "/com.flamedev.flowyapphub.gresource";
        var weirdPath = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)) + "/com.flamedev.flowyapphub.gresource";
        var res = Functions.ResourceLoad(File.Exists(mainResPath) ? mainResPath : resPath);
        Functions.ResourcesRegister(res);
        ResourceContentDebug(res);

        var theme = Gtk.IconTheme.GetForDisplay(Gdk.Display.GetDefault());
        theme.AddResourcePath("/com/flamedev/flowyapphub/icons");
        theme.AddSearchPath("icons");
        // theme.AddResourcePath("/var/lib/flatpak/exports/share/icons");
        FlatpakDirs.LocalFlatpak.FlatpakIconsFolderExists(AddFlatpakLocalIcons);
        FlatpakDirs.GlobalFlatpak.FlatpakIconsFolderExists(AddFlatpakGlobalIcons);
        
        // theme.AddSearchPath("/var/lib/flatpak/exports/share/icons");
        // theme.AddSearchPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local/share/flatpak/exports/share/icons"));
        
        
        if (!File.Exists(weirdPath))
        {
            Console.WriteLine($"GResource file not found at: {weirdPath}");
        }

        IconThemeDebug(theme);
    }

    private static void AddFlatpakLocalIcons()
    {
        var theme = IconTheme.GetForDisplay(Gdk.Display.GetDefault()!);
        theme.AddSearchPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local/share/flatpak/exports/share/icons"));
    }

    private static void AddFlatpakGlobalIcons()
    {
        var theme = IconTheme.GetForDisplay(Gdk.Display.GetDefault()!);
        theme.AddSearchPath("/var/lib/flatpak/exports/share/icons");
    }

    private static void ResourceContentDebug(Resource res)
    {
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
    }

    private static void IconThemeDebug(IconTheme theme)
    {
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
    }
}