namespace BlazeHub.Services.Flatpak.Dirs;

public class FlatpakDirs(string basePath)
{
    public static FlatpakDirs LocalFlatpak { get; } = new(LocalFlatpakDir);
    public static FlatpakDirs GlobalFlatpak { get; } = new("/var/lib/flatpak");

    private static string LocalFlatpakDir =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".local/share/flatpak");
    
    private string FlatpakDir => 
        basePath;
    /// <summary>
    /// '~.../flatpak/app' path
    /// </summary>
    private string FlatpakApp =>
        Path.Combine(FlatpakDir, "app");
    private string FlatpakExports =>
        Path.Combine(FlatpakDir, "exports");
    private string FlatpakShare =>
        Path.Combine(FlatpakExports, "share");
    private string FlatpakIcons =>
        Path.Combine(FlatpakShare, "icons");

    private FileSystemWatcher? _watcher;

    /// <summary>
    /// Checks if '.../flatpak/app' exists. If exists, callback will be invoked immediately, if not - when the directory will be created.
    /// </summary>
    /// <param name="callback">Callback</param>
    /// <returns>If directory exists or not.</returns>
    public bool FlatpakAppsFolderExists(Action callback)
        => CheckFlatpakDirectory(callback, FlatpakApp);

    /// <summary>
    /// Checks if '.../flatpak/exports/share/icons' exists. If exists, callback will be invoked immediately, if not - when the directory will be created.
    /// </summary>
    /// <param name="callback">Callback which is invoked in case, if directory is created.</param>
    /// <returns>If directory exists or not.</returns>
    public bool FlatpakIconsFolderExists(Action callback)
        => CheckFlatpakDirectory(callback, FlatpakIcons);
    

    private bool CheckFlatpakDirectory(Action callback, string directory)
    {
        var exists = Directory.Exists(directory);
        try
        {
            if (exists)
            {
                callback.Invoke();
                return exists;
            }
            if (_watcher == null)
            {
                _watcher = new FileSystemWatcher(FlatpakDir);
                ConfigureWatcher(_watcher);
            }
            _watcher.Created += CallbackWrapper;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return exists;

        void CallbackWrapper(object obj, FileSystemEventArgs args)
        {
            callback?.Invoke();
            if (_watcher != null && args.FullPath.StartsWith(directory))
                _watcher.Created -= CallbackWrapper;
        }
    }
    
    private static void ConfigureWatcher(FileSystemWatcher watcher)
    {
        watcher.NotifyFilter = NotifyFilters.Attributes
                               | NotifyFilters.CreationTime
                               | NotifyFilters.DirectoryName
                               | NotifyFilters.FileName
                               | NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.Security
                               | NotifyFilters.Size;
        watcher.Filter = "*.*";
        watcher.EnableRaisingEvents = true;
        watcher.IncludeSubdirectories = true;
    }
}