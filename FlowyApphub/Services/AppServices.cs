using FlowyApphub.Services.Flatpak;

namespace FlowyApphub.Services;

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