using FlowyApphub.Models.Flathub;

namespace FlowyApphub.Services.Flathub.Safety;

public static class FlathubSafety
{
    public static List<FlathubSafetyFeature> GetAppSafetyFeatures(FlathubAppPermissions permissions)
    {
        var features = new List<FlathubSafetyFeature>();
        
        features.Add(new FlathubSafetyFeature("windowing", "Legacy Windowing System", true, permissions.Sockets.ToArray(),
            (_, keywords) => !keywords.Contains("wayland"),
            (_, _) => "Uses legacy windowing system.",
            (_, keywords) => !keywords.Contains("wayland") ? 0 : 2));
        
        features.Add(new FlathubSafetyFeature("network", "Network access",
    permissions.Shared.Contains("network"),
    ["network"],
            (present, _) => present,
            (present, _) => present ? "Has network access" : "Has not network access",
            (present, _) => present ? 1 : 2
        ));
        
        return features;
    }

    public static List<string> GetAppSafetyShortFeatures(List<FlathubSafetyFeature> features)
    {
        var safetyScore = GetAppSafety(features);
        var shortFeatures = new List<string>();

        foreach (var feature in features.Where(f => f.SafetyScore == safetyScore))
        {
            shortFeatures.Add(feature.Name);
        }
        
        return shortFeatures;
    }
    
    public static int GetAppSafety(List<FlathubSafetyFeature> features)
    {
        // // EXCEPTIONS
        // var appDanger = features.Find(f => f.ID == "windowing")?.SafetyScore ?? 0;
        //
        // if (appDanger == 0)
        //     return appDanger;

        var appSafetyScore = 3;

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