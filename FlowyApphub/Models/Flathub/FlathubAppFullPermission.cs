namespace FlowyApphub.Models.Flathub;

public class FlathubAppFullPermission(string keyword, string title, string description)
{
    public string Keyword { get; set; }
    public string Title { get; set; } = title;
    public string Description { get; set; } = description;
}