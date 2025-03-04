using BlazeHub.Services.Dialog;

namespace BlazeHub.Services.Flatpak;

public static class FlatpakListener
{
    public delegate void FlatpakChangedArgs();
    public static event FlatpakChangedArgs? OnFlatpakFolderChanged;
    
    private static FileSystemWatcher _watcher = null!;
    
    public const string FLATPAK_APPS_PATH = "/var/lib/flatpak/app";

    public static async void StartWatcher()
    {
        try
        {
            Console.WriteLine("Starting Flatpak Watcher...");
            _watcher = new FileSystemWatcher(FLATPAK_APPS_PATH);
            _watcher.NotifyFilter = NotifyFilters.Attributes
                                   | NotifyFilters.CreationTime
                                   | NotifyFilters.DirectoryName
                                   | NotifyFilters.FileName
                                   | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.Security
                                   | NotifyFilters.Size;
            // _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;
            _watcher.Error += OnError;
            
            _watcher.Filter = "*.*";
            // watcher.IncludeSubdirectories = true;
            _watcher.EnableRaisingEvents = true;
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
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