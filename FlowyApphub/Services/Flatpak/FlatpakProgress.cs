namespace FlowyApphub.Services.Flatpak;

public class FlatpakProgress
{
    public List<FlatpakPackageProgress> PackagesProgress { get; } = [];
    
    public float Timer { get; set; }

    public double Progress => PackagesProgress.Sum(p => p.SizeBytes * p.Progress) / OverallSizeBytes;
    public int CurrentStage { get; set; } = 0;
    public ulong OverallSizeBytes => (ulong)PackagesProgress.Sum(p => p.SizeBytes);
    public string CurrentNetworkSpeed { get; set; } = string.Empty;
    public ulong EstimatedDownloadedSize => (ulong)PackagesProgress.Sum(p => (double)p.EstimatedDownloadedSize);
}