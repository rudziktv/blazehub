namespace BlazeHub.Models.Flatpak;

public class InstalledFlatpakApp(string id, string name, string version, string branch, string origin, string installation, string installedSize) : FlatpakApp(id, name, version, branch, origin, installation)
{
    public string InstalledSize => installedSize;
}