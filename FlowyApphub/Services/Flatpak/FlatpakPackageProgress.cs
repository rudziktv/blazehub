using FlowyApphub.Utils;

namespace FlowyApphub.Services.Flatpak;

public partial class FlatpakPackageProgress(string id, string branch, string op, string source, string size)
{
    public string PackageID => id;
    public string Branch => branch;
    public string Operation => op;
    public string Source => source;
    public string Size => size;

    public uint SizeBytes => NetworkUnitUtils.SizeToBytes(Size);

    public float Progress { get; set; } = 0f;
    public ulong EstimatedDownloadedSize { get; set; } = 0;
}