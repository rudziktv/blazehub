namespace FlowyApphub.Models.Flathub;

[Serializable]
public class FlathubAppSummaryMeta
{
    public string Sdk { get; set; }
    public string Name { get; set; }
    public string Command { get; set; }
    public string Runtime { get; set; }

    // public FlathubAppExtensions Extensions { get; set; }
    public FlathubAppPermissions Permissions { get; set; }
}

// [Serializable]
// public class FlathubAppExtensions
// {
//     
// }

[Serializable]
public class FlathubAppPermissions
{
    public List<string> Shared { get; set; }
    public List<string> Devices { get; set; }
    public List<string> Sockets { get; set; }
    public List<string>? Filesystems { get; set; }
}