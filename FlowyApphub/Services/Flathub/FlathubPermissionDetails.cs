using BlazeHub.Models.Flathub;

namespace BlazeHub.Services.Flathub;

public static class FlathubPermissionDetails
{
    public static readonly FlathubAppFullPermission Network = new("network", "Network access", "Has network access");
    
    public static FlathubAppFullPermission GetPermissionDetails(string keyword)
    {
        if (keyword == "network")
            return Network;

        return new FlathubAppFullPermission(keyword, "unknown", "no description - unknown");
    }
}