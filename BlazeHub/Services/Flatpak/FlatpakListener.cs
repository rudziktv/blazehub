using BlazeHub.Services.Dialog;

namespace BlazeHub.Services.Flatpak;

public static class FlatpakListener
{
    public delegate void FlatpakChangedArgs();
    public static event FlatpakChangedArgs? OnFlatpakFolderChanged;
    
    private static FileSystemWatcher _watcher = null!;
    private static FileSystemWatcher _localWatcher = null!;
    
    public const string FLATPAK_APPS_PATH = "/var/lib/flatpak/app";
    public static string LocalFlatpakAppsPath => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
        ".local/share/flatpak/app");

    public static async void StartWatcher()
    {
        try
        {
            Console.WriteLine("Starting Flatpak Watcher...");
            _watcher = new FileSystemWatcher(FLATPAK_APPS_PATH);
            _localWatcher = new FileSystemWatcher(LocalFlatpakAppsPath);

            _watcher.ConfigureWatcher();
            _localWatcher.ConfigureWatcher();
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }

    private static void ConfigureWatcher(this FileSystemWatcher watcher)
    {
        watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;
        watcher.Created += OnCreated;
        watcher.Deleted += OnDeleted;
        watcher.Renamed += OnRenamed;
        watcher.Error += OnError;
            
        watcher.Filter = "*.*";
        watcher.EnableRaisingEvents = true;
    }

    private static void OnCreated(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Created: {e.FullPath}");
        OnFlatpakFolderChanged?.Invoke();
        // Invoke();
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Deleted: {e.FullPath}");
        OnFlatpakFolderChanged?.Invoke();
        // Invoke();
    }

    private static void OnRenamed(object sender, RenamedEventArgs e)
    {
        Console.WriteLine($"Renamed:");
        Console.WriteLine($"    Old: {e.OldFullPath}");
        Console.WriteLine($"    New: {e.FullPath}");
    }

    private static void OnError(object sender, ErrorEventArgs e) =>
        ErrorDialogService.ShowErrorDialog(e.GetException());
    
    
}