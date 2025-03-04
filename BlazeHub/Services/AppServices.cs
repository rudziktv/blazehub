using BlazeHub.Services.Flatpak;

namespace BlazeHub.Services;

public static class AppServices
{
    public static void StartAppServices()
    {
        Console.WriteLine("Starting App Services");
        FlatpakService.InitializeFlatpakService();
        FlatpakListener.StartWatcher();
        Console.WriteLine("App Services Started");
    }
}