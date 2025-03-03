namespace FlowyApphub.Models.Flathub;

public class FlathubFullApp(FlathubAppModel appstream, FlathubAppSummary appSummary)
{
    public FlathubAppModel Appstream { get; private set; } = appstream;
    public FlathubAppSummary Summary { get; private set; } = appSummary;
}