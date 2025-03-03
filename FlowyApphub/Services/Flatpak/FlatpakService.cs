using System.Diagnostics;
using System.Text.RegularExpressions;
using FlowyApphub.Models.Flatpak;
using FlowyApphub.Services.Dialog;
using Gio;
using File = System.IO.File;
using Monitor = Gdk.Monitor;
using Task = System.Threading.Tasks.Task;

namespace FlowyApphub.Services.Flatpak;

public static class FlatpakService
{
    private static InstalledFlatpakApp[] _installedFlatpakApps = [];
    
    private static CancellationTokenSource TokenSource { get; set; } = new();
    private static Task? CurrentRefreshTask { get; set; }
    public static bool IsRefreshing { get; private set; }
    public static InstalledFlatpakApp[] InstalledFlatpakApps
    {
        get => _installedFlatpakApps;
        private set
        {
            _installedFlatpakApps = value;
            InstalledFlatpakIDs = value.Select(a => a.ID).ToArray();
        }
    }
    public static string[] InstalledFlatpakIDs { get; private set; } = [];

    public static Dictionary<string, InstalledFlatpakApp> InstalledFlatpaks { get; private set; } = [];
    public static event FlatpakListener.FlatpakChangedArgs? OnFlatpakChangeReceived;
    public static event FlatpakListener.FlatpakChangedArgs? OnInstalledAppsChanged;
    
    
    
    
    // TODO - monitor flatpak changes
    public static async void InitializeFlatpakService()
    {
        try
        {
            await FlatpakHelloWorld();
            CurrentRefreshTask = RefreshFlatpakAppListTask();
            FlatpakListener.OnFlatpakFolderChanged += FlatpakFolderChanged;
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }

    private static void FlatpakFolderChanged()
    {
        try
        {
            RefreshFlatpakAppList();
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }

    public static async void RefreshFlatpakAppList()
    {
        try
        {
            await TokenSource.CancelAsync();
            if (!TokenSource.TryReset())
                TokenSource = new CancellationTokenSource();
        
            OnFlatpakChangeReceived?.Invoke();
            await RefreshFlatpakAppListTask(TokenSource.Token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static async Task RefreshFlatpakAppListTask(CancellationToken token = default)
    {
        try
        {
            IsRefreshing = true;
            await Task.Delay(500, token);
            if (token.IsCancellationRequested)
                return;
            var apps = await GetFlatpakAppList(token);
            SetFlatpakAppList(apps);
            if (token.IsCancellationRequested)
                return;
            IsRefreshing = false;
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }
    
    private static void SetFlatpakAppList(List<InstalledFlatpakApp> flatpakApps)
        => SetFlatpakAppList(flatpakApps.ToArray());

    private static void SetFlatpakAppList(InstalledFlatpakApp[] flatpakApps)
    {
        InstalledFlatpakApps = flatpakApps;
        foreach (var app in flatpakApps)
        {
            InstalledFlatpaks[app.ID] = app;
        }
        OnInstalledAppsChanged?.Invoke();
    }

    private static ProcessStartInfo GetFlatpakStartInfo(string args)
    {
        Console.WriteLine($"FLATPAK_CMD = {Environment.GetEnvironmentVariable("FLATPAK_CMD")}");
        if (Environment.GetEnvironmentVariable("FLATPAK_CMD") != null)
            return new ProcessStartInfo
            {
                FileName = "flatpak-spawn",
                Arguments = $"--host flatpak {args}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
        return new ProcessStartInfo
        {
            FileName = "flatpak",
            Arguments = args,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
    }

    public static async Task<bool> UninstallApp(string appId)
    {
        try
        {
            // var psi = new ProcessStartInfo
            // {
            //     FileName = "flatpak",
            //     Arguments = $"uninstall --app -y {appId}",
            //     RedirectStandardOutput = true,
            //     UseShellExecute = false,
            //     CreateNoWindow = true
            // };
            var info = GetFlatpakStartInfo($"uninstall --app -y {appId}");
            using var process = new Process();
            process.StartInfo = info;
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

    private static async Task FlatpakHelloWorld()
    {
        try
        {
            Console.WriteLine("flatpak helloworld");
            var info = GetFlatpakStartInfo("--version");
            using Process process = new Process();
            process.StartInfo = info;
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync();
            Console.WriteLine(output);
            await process.WaitForExitAsync();
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private static async Task<List<InstalledFlatpakApp>> GetFlatpakAppList(CancellationToken token = default)
    {
        List<InstalledFlatpakApp> apps = [];
        try
        {
            // old way
            // ProcessStartInfo info = new ProcessStartInfo
            // {
            //     // FileName = "flatpak", // old, dev working
            //     FileName = Environment.GetEnvironmentVariable("FLATPAK_CMD") ?? "flatpak",
            //     Arguments = "list --app --columns=application,name,version,branch,origin,installation,size",
            //     RedirectStandardOutput = true,
            //     UseShellExecute = false,
            //     CreateNoWindow = true
            // };
            Console.WriteLine("Getting Flatpak AppList");
            var info = GetFlatpakStartInfo("list --app --columns=application,name,version,branch,origin,installation,size");
            using Process process = new Process();
            process.StartInfo = info;
            process.Start();

            var output = await process.StandardOutput.ReadToEndAsync(token);
            await process.WaitForExitAsync(token);
            if (token.IsCancellationRequested)
                return apps;

            var outputApps = output.Split('\n');


            foreach (var app in outputApps)
            {
                var appDetails = app.Split("\t");
                if (appDetails.Length != 7) continue;
                if (token.IsCancellationRequested) return [];
            
                apps.Add(new InstalledFlatpakApp(appDetails[0], appDetails[1], appDetails[2], appDetails[3], appDetails[4], appDetails[5], appDetails[6]));
                // Console.WriteLine(Regex.Replace(app, @"\s+", " ").Trim());
            }
        
            Console.WriteLine("Installed flatpak apps: " + apps.Count);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        
        return apps;
    }
}