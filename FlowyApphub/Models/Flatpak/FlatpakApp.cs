namespace FlowyApphub.Models.Flatpak;

public class FlatpakApp(string id, string name, string version, string branch, string origin, string installation)
{
    public string ID => id;
    public string Name => name;
    public string Version => version;
    public string Branch => branch;
    public string Origin => origin;
    public string Installation => installation;
}