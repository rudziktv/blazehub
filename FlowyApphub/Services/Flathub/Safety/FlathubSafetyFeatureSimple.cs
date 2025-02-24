namespace FlowyApphub.Services.Flathub.Safety;

public class FlathubSafetyFeatureSimple(
    string id, string name, int score, string description)
    : IFlathubSafetyFeature
{
    public string ID => id;
    public string Name => name;
    public int SafetyScore => score;
    public string Description => description;
}