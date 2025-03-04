using BlazeHub.Models.Flathub;

namespace BlazeHub.Services.Flathub.Safety;

public static class FlathubSafetyGenerator
{
    
    public static IFlathubSafetyFeature GetNetworkSafety(FlathubAppPermissions permissions)
    {
        var netExists = permissions.Shared.Contains("network");

        var feature = new FlathubSafetyFeatureSimple("network", "Network access",
            netExists ? 1 : 2,
            netExists ? "Has network access" : "Has not network access",
            netExists);
        return feature;
    }
}