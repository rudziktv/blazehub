using FlowyApphub.Services.Dialog;
using Tmds.DBus;

namespace FlowyApphub.Services.Flatpak;

// TODO! use:
// inotifywait -m /var/lib/flatpak/app -e delete -e create
// inotifywait is not natievly on Fedora!
// args:
// m -> monitor
// e -> event

public static class FlatpakListener
{
    private const string FLATPAK_DBUS_NAME = "org.freedesktop.Flatpak";
    private const string FLATPAK_DBUS_PATH = "org/freedesktop/flatpak";
    
    private static readonly Connection DbusConnection;
    private static IFlatpakMonitor _flatpakMonitor;

    static FlatpakListener()
    {
        DbusConnection = new Connection(Address.Session);
        Initialize();
    }

    private static async void Initialize()
    {
        try
        {
            await DbusConnection.ConnectAsync();
            _flatpakMonitor = DbusConnection.CreateProxy<IFlatpakMonitor>(FLATPAK_DBUS_NAME, FLATPAK_DBUS_PATH);
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }
}