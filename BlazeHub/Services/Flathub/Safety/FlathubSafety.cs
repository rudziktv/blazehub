using BlazeHub.Models.Flathub;

namespace BlazeHub.Services.Flathub.Safety;

public static class FlathubSafety
{
    public static List<IFlathubSafetyFeature> GetAppSafetyFeatures(FlathubAppPermissions permissions)
    {
        var features = new List<IFlathubSafetyFeature>();
        
        features.Add(new FlathubSafetyFeature("windowing", "Legacy Windowing System", true, permissions.Sockets.ToArray(),
            (_, keywords) => !keywords.Contains("wayland"),
            (_, _) => "Uses legacy windowing system.",
            (_, keywords) => !keywords.Contains("wayland") ? 0 : 2));
    
        features.Add(FlathubSafetyGenerator.GetNetworkSafety(permissions));
        
        return features;
    }

    public static List<string> GetAppSafetyShortFeatures(List<IFlathubSafetyFeature> features)
    {
        var safetyScore = GetAppSafetyScore(features);
        var shortFeatures = new List<string>();

        foreach (var feature in features.Where(f => f.SafetyScore == safetyScore && f.Shown))
        {
            shortFeatures.Add(feature.Name);
        }
        
        return shortFeatures;
    }
    
    public static int GetAppSafetyScore(List<IFlathubSafetyFeature> features)
    {
        var appSafetyScore = 2;

        foreach (var safetyFeature in features)
        {
            if (safetyFeature.SafetyScore < appSafetyScore)
                appSafetyScore = safetyFeature.SafetyScore;
            if (appSafetyScore == 0)
                break;
        }
        
        return appSafetyScore;
    }
}