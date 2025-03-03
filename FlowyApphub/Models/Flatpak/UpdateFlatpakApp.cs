namespace FlowyApphub.Models.Flatpak;

public class UpdateFlatpakApp(string newVersion, string downloadSize)
{
    public string NewVersion => newVersion;
    public string DownloadSize => downloadSize;
}