using FlowyApphub.Services.Flatpak;

namespace FlowyApphub.Services;

public static class AppServices
{
    public static void StartAppServices()
    {
        FlatpakService.InitializeFlatpakService();
        FlatpakListener.StartWatcher();
    }
}