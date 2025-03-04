namespace BlazeHub.Services.Flathub.Safety;

public class FlathubSafetyFeature(string id, string name, bool present, string[] keywords,
    FlathubSafetyFeature.ShowCallback showCallback,
    FlathubSafetyFeature.DescriptionCallback descriptionCallback,
    FlathubSafetyFeature.SafetyScoreCallback scoreCallback,
    FlathubSafetyFeature.NameCallback? nameCallback = null)
    : IFlathubSafetyFeature
{
    public delegate bool ShowCallback(bool present, string[] keywords);
    public delegate int SafetyScoreCallback(bool present, string[] keywords);
    public delegate string NameCallback(bool present, string[] keywords);
    public delegate string DescriptionCallback(bool present, string[] keywords);

    public string ID => id;

    public string Name => nameCallback != null ? nameCallback(present, keywords) : name;
    public bool Present => present;
    public string[] Keywords => keywords;
    public bool Shown => showCallback(Present, Keywords);
    public int SafetyScore => scoreCallback(Present, Keywords);
    public string Description => descriptionCallback(Present, Keywords);
}