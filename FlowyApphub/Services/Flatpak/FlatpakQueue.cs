using System.Diagnostics;
using System.Text.RegularExpressions;
using FlowyApphub.Utils;

namespace FlowyApphub.Services.Flatpak;

public static class FlatpakQueue
{
    private static Task? CurrentTask;
    private static Stopwatch _stopwatch = new();
    public static FlatpakProgress? CurrentProgress { get; private set; }

    public static event Action<FlatpakPackageProgress>? OnFlatpakPackageAttached;
    public static event Action? OnTaskProgress;
    public static event Action<FlatpakAction>? OnTaskStarted;
    public static event Action<FlatpakAction>? OnTaskFinished;
    public static event Action? OnQueueChanged;
    
    public static List<FlatpakAction> Queue { get; private set; } = [];

    
    public static void AddToQueue(FlatpakAction action)
    {
        Queue.Add(action);
        OnQueueChanged?.Invoke();
        StartTaskIfFree();
    }

    public static void AddToQueue(FlatpakAction[] actions)
    {
        Queue.AddRange(actions);
        OnQueueChanged?.Invoke();
        StartTaskIfFree();
    }

    public static void RemoveFromQueue(FlatpakAction action)
    {
        var index = Queue.IndexOf(action);
        if (index != 0)
            Queue.Remove(action);
        else // TODO - force stop on Task
            Console.WriteLine("Action already started...");
    }

    private static void StartTaskIfFree()
    {
        CurrentTask ??= ProcessTask();
    }

    private static async Task ProcessTask(CancellationToken token = default)
    {
        try
        {
            if (Queue.Count == 0)
                return;
            
            OnTaskStarted?.Invoke(Queue[0]);
            switch (Queue[0].ActionType)
            {
                case FlatpakActionType.Install:
                    await ProcessInstallTask(Queue[0].AppTarget, token);
                    break;
            }
            OnTaskFinished?.Invoke(Queue[0]);
            Queue.RemoveAt(0);
            if (Queue.Count != 0)
                CurrentTask = ProcessTask(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    static void ProcessOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
    {
        var line = outLine.Data;
        if (string.IsNullOrEmpty(line) || CurrentProgress == null) return;
        
        
        if (line.Contains(".\t"))
        {
            var reg = new Regex(@"(\d+\.)\s+([\w\.]+)\s+(\w+)\s+(\w+)\s+(\w+)\s+.\s+([\d\.]+.\w+)(?:\s+(\D+))?");
            var mat = reg.Match(line);
            if (!mat.Success) return;
            var package = new FlatpakPackageProgress(
                mat.Groups[2].Value, mat.Groups[3].Value,
                mat.Groups[4].Value, mat.Groups[5].Value, mat.Groups[6].Value);
            CurrentProgress.PackagesProgress.Add(package);
            OnFlatpakPackageAttached?.Invoke(package);
            _stopwatch.Restart();
        }
        
        if (line.Contains("Installing"))
        {
            string pattern = @"Installing(?:\s+(\d+\/\d+))?.*?(\d+%)(?:\s+([\d.,]+\s*\S+\/s))?(?:\s+(\d+:\d+))?";
        
            Regex regex = new Regex(pattern);
            var match = regex.Match(line);
            
            if (!match.Success) return;
            
            int stage = match.Groups[1].Success ? int.Parse(match.Groups[1].Value.Split("/")[0]) - 1 : 0;      // "2/2"
            string percent = match.Groups[2].Value;      // "94%"
            string speed = match.Groups[3].Success ? match.Groups[3].Value : "0 bytes/s";        // "1.1 MB/s"
            // string timeLeft = match.Groups[4].Success ? match.Groups[4].Value : ""; // "00:02"
    
            CurrentProgress.CurrentStage = stage;
            CurrentProgress.CurrentNetworkSpeed = speed;
            
            var package = CurrentProgress.PackagesProgress[stage];

            if (int.TryParse(percent.Replace(@"%", ""), out var percentInt))
                package.Progress = Math.Clamp(percentInt / 100f, 0, 1);
            
            var downloadDelta = (ulong)(NetworkUnitUtils.SpeedToBytes(speed) * _stopwatch.Elapsed.TotalSeconds);
            var downloadMax = NetworkUnitUtils.SizeToBytes(package.Size) * (double)package.Progress;

            package.EstimatedDownloadedSize = (ulong)Math.Min(package.EstimatedDownloadedSize + downloadDelta, downloadMax);
            
            OnTaskProgress?.Invoke();
        }
        _stopwatch.Restart();
    }

    private static async Task<bool> ProcessUninstallTask(string appId, CancellationToken token = default)
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
            await process.WaitForExitAsync(token);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }

    private static async Task<bool> ProcessInstallTask(string appId, CancellationToken token = default)
    {
        try
        {
            var psi = new ProcessStartInfo
            {
                FileName = "flatpak",
                Arguments = $"install --app -y flathub {appId}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using var process = new Process();
            process.StartInfo = psi;
            process.Start();

            CurrentProgress = new FlatpakProgress();
            // process.StandardOutput.DiscardBufferedData();
            process.OutputDataReceived += ProcessOutputHandler;
            process.BeginOutputReadLine();
            _stopwatch.Start();
            await process.WaitForExitAsync(token);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
    
}