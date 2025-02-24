namespace FlowyApphub.Services.Flathub.Safety;

public interface IFlathubSafetyFeature
{
    public string ID { get; }
    public string Name { get; }
    public int SafetyScore { get; }
}