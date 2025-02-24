using System.Diagnostics;
using System.Text.RegularExpressions;
using FlowyApphub.Models.Flatpak;
using Gio;
using File = System.IO.File;
using Monitor = Gdk.Monitor;

namespace FlowyApphub.Services.Flatpak;

public static class FlatpakService
{
    static FlatpakService()
    {
        StartMonitoringFlatpak();
    }
    
    // TODO - monitor flatpak changes
    static void StartMonitoringFlatpak()
    {
        Console.WriteLine("Starting monitoring flatpak");
        string path = "/var/lib/flatpak/app"; // Lub ~/.local/share/flatpak/app dla użytkownika

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Ścieżka nie istnieje! Sprawdź, czy Flatpak jest zainstalowany.");
            return;
        }

        using var watcher = new FileSystemWatcher(path)
        {
            NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.CreationTime,
            IncludeSubdirectories = true, // WAŻNE!
            EnableRaisingEvents = true    // WAŻNE!
        };

        watcher.Created += (s, e) => Console.WriteLine($"Zainstalowano: {e.FullPath}");
        watcher.Deleted += (s, e) => Console.WriteLine($"Odinstalowano: {e.FullPath}");
        watcher.Changed += (s, e) => Console.WriteLine($"Zmieniono: {e.FullPath}");
        watcher.Renamed += (s, e) => Console.WriteLine($"Zmieniono nazwę: {e.OldFullPath} → {e.FullPath}");
    }
    
    public static async Task<bool> UninstallApp(string appId)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "flatpak",
                Arguments = $"uninstall --app -y {appId}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = psi;
            process.Start();
            // var output = await process.StandardOutput.ReadToEndAsync();
            // Console.WriteLine(output);
            await process.WaitForExitAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
    
    public static async Task<List<InstalledFlatpakApp>> GetAppsList()
    {
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = "flatpak",
            Arguments = "list --app --columns=application,name,version,branch,origin,installation,size",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using Process process = new Process();
        process.StartInfo = psi;
        process.Start();

        var output = await process.StandardOutput.ReadToEndAsync();
        await process.WaitForExitAsync();

        var outputApps = output.Split('\n');

        List<InstalledFlatpakApp> apps = [];

        foreach (var app in outputApps)
        {
            var appDetails = app.Split("\t");
            if (appDetails.Length != 7) continue;
            
            apps.Add(new InstalledFlatpakApp(appDetails[0], appDetails[1], appDetails[2], appDetails[3], appDetails[4], appDetails[5], appDetails[6]));
            Console.WriteLine(Regex.Replace(app, @"\s+", " ").Trim());
        }
        
        Console.WriteLine("Installed flatpak apps: " + apps.Count);
        
        return apps;
    }
}