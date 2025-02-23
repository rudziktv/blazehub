using System.Text.Json.Serialization;

namespace FlowyApphub.Models.Flathub;

[Serializable]
public class FlathubAppSummary
{
    public List<string> Arches { get; set; }
    public string Branch { get; set; }

    public FlathubAppSummaryMeta Metadata { get; set; }
    public int Timestamp { get; set; }
    [JsonPropertyName("download_size")] public int DownloadSize { get; set; }
    [JsonPropertyName("installed_size")] public int InstalledSize { get; set; }
}