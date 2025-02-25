using Tmds.DBus;

namespace FlowyApphub.Services.Flatpak;

public class IFlatpakMonitor : IDBusObject
{
    public ObjectPath ObjectPath { get; }
}