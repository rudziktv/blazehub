using System.Text.Json.Serialization;

namespace FlowyApphub.Models.FlathubApp;

[Serializable]
public class FlathubAppModel
{
    public string Id { get; set; }
    public string Icon { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public FlathubUrlsModel Urls { get; set; }
    public List<FlathubIconModel> Icons { get; set; }
    public FlathubBundleModel Bundle { get; set; }
    public string Summary { get; set; }
    public List<Branding> Branding { get; set; }
    public List<string> Keywords { get; set; }
    public FlathubAppMetadataModel Metadata { get; set; }
    public List<string> Provides { get; set; }
    public List<FlathubAppReleaseModel> Releases { get; set; }
    public List<string> Requires { get; set; }
    public List<FlathubAppLanguageModel> Languages { get; set; }
    public List<string> Categories { get; set; }
    public List<string> Developers { get; set; }
    public FlathubLaunchableModel Launchable { get; set; }
    // public List<string> Recommends { get; set; } // not able to define
    public string Description { get; set; }
    public List<Screenshot> Screenshots { get; set; }
    
    [JsonPropertyName("content_rating")] public FlathubContentRatingModel ContentRating { get; set; }
    [JsonPropertyName("developer_name")] public string DeveloperName { get; set; }
    [JsonPropertyName("is_free_license")] public bool IsFreeLicense { get; set; }
    [JsonPropertyName("project_license")] public string ProjectLicense { get; set; }
    public bool IsMobileFriendly { get; set; }
}

[Serializable]
public class FlathubUrlsModel
{
    public string Homepage { get; set; }
    public string Bugtracker { get; set; }
}

[Serializable]
public class FlathubIconModel
{
    public string Url { get; set; }
    public string Type { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
    public string Scale { get; set; } // Optional
}

[Serializable]
public class FlathubBundleModel
{
    public string Sdk { get; set; }
    public string Type { get; set; }
    public string Value { get; set; }
    public string Runtime { get; set; }
}

[Serializable]
public class Branding
{
    public string Type { get; set; }
    public string Value { get; set; }
    public string SchemePreference { get; set; }
}

[Serializable]
public class FlathubAppMetadataModel
{
    public string FormFactor { get; set; }
    public string BuildLogUrl { get; set; }
    public string VerificationMethod { get; set; }
    public string Verified { get; set; }
    public string VerificationTimestamp { get; set; }
    public string LoginName { get; set; }
    public string LoginProvider { get; set; }
    public bool LoginIsOrganization { get; set; }
}

[Serializable]
public class FlathubAppReleaseModel
{
    public string Type { get; set; }
    public string Version { get; set; }
    public string Timestamp { get; set; }
    public string Description { get; set; }
}

[Serializable]
public class FlathubAppLanguageModel
{
    public string Value { get; set; }
    public string Percentage { get; set; }
}

[Serializable]
public class FlathubLaunchableModel
{
    public string Type { get; set; }
    public string Value { get; set; }
}

[Serializable]
public class Screenshot
{
    public List<Size> Sizes { get; set; }
    public string Caption { get; set; }
    public bool Default { get; set; } // Optional
}

[Serializable]
public class Size
{
    public string Src { get; set; }
    public string Scale { get; set; }
    public string Width { get; set; }
    public string Height { get; set; }
}

[Serializable]
public class FlathubContentRatingModel
{
    public string Type { get; set; }
}
