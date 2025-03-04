using BlazeHub.Services.Dialog;
using BlazeHub.Services.Flatpak.Dirs;

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

    public static async void InitializeListener()
    {
        try
        {
            FlatpakDirs.LocalFlatpak.FlatpakAppsFolderExists(StartLocalWatcher);
            FlatpakDirs.GlobalFlatpak.FlatpakAppsFolderExists(StartGlobalWatcher);
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }

    private static void StartGlobalWatcher()
    {
        try
        {
            Console.WriteLine("Starting global watcher");
            _watcher = new FileSystemWatcher(FLATPAK_APPS_PATH);
            _watcher.ConfigureWatcher();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ErrorDialogService.ShowErrorDialog(e);
        }
    }

    private static void StartLocalWatcher()
    {
        try
        {
            Console.WriteLine("Starting local watcher");
            _localWatcher = new FileSystemWatcher(LocalFlatpakAppsPath);
            _localWatcher.ConfigureWatcher();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            ErrorDialogService.ShowErrorDialog(e);
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
    }

    private static void OnDeleted(object sender, FileSystemEventArgs e)
    {
        Console.WriteLine($"Deleted: {e.FullPath}");
        OnFlatpakFolderChanged?.Invoke();
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