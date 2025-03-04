namespace BlazeHub.Services.Flatpak;

public class FlatpakRemote(string name, string options)
{
    public string Name => name;
    public string Options => options;
}