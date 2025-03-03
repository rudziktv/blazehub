using System.Text.Json.Serialization;

namespace FlowyApphub.Models.Flathub;

[Serializable]
public class FlathubAppModel
{
    public required string Id { get; set; }
    public required string Icon { get; set; }
    public required string Name { get; set; }
    public required string Type { get; set; }
    public required FlathubUrlsModel Urls { get; set; }
    public required List<FlathubIconModel> Icons { get; set; }
    public required FlathubBundleModel Bundle { get; set; }
    public required string Summary { get; set; }
    public List<Branding>? Branding { get; set; }
    public List<string>? Keywords { get; set; }
    public required FlathubAppMetadataModel Metadata { get; set; }
    // public List<string> Provides { get; set; }
    public required List<FlathubAppReleaseModel> Releases { get; set; }
    public List<string>? Requires { get; set; }
    public List<FlathubAppLanguageModel>? Languages { get; set; }
    public List<string>? Categories { get; set; }
    public List<string>? Developers { get; set; }
    public required FlathubLaunchableModel Launchable { get; set; }
    // public List<string> Recommends { get; set; } // not able to define
    public required string Description { get; set; }
    public required List<Screenshot> Screenshots { get; set; }
    
    [JsonPropertyName("content_rating")] public required FlathubContentRatingModel ContentRating { get; set; }
    [JsonPropertyName("developer_name")] public required string DeveloperName { get; set; }
    [JsonPropertyName("is_free_license")] public bool IsFreeLicense { get; set; }
    [JsonPropertyName("project_license")] public required string ProjectLicense { get; set; }
    public bool IsMobileFriendly { get; set; }
}

[Serializable]
public class FlathubUrlsModel
{
    public string? Homepage { get; set; }
    public string? Bugtracker { get; set; }
}

[Serializable]
public class FlathubIconModel
{
    public required string Url { get; set; }
    public required string Type { get; set; }
    public required string Width { get; set; }
    public required string Height { get; set; }
    public string? Scale { get; set; } // Optional
}

[Serializable]
public class FlathubBundleModel
{
    public required string Sdk { get; set; }
    public required string Type { get; set; }
    public required string Value { get; set; }
    public required string Runtime { get; set; }
}

[Serializable]
public class Branding
{
    public required string Type { get; set; }
    public required string Value { get; set; }
    public string? SchemePreference { get; set; }
}

[Serializable]
public class FlathubAppMetadataModel
{
    public string? FormFactor { get; set; }
    public string? BuildLogUrl { get; set; }
    public string? VerificationMethod { get; set; }
    public string? Verified { get; set; }
    public string? VerificationTimestamp { get; set; }
    public string? LoginName { get; set; }
    public string? LoginProvider { get; set; }
    public bool LoginIsOrganization { get; set; }
}

[Serializable]
public class FlathubAppReleaseModel
{
    public required string Type { get; set; }
    public required string Version { get; set; }
    public required string Timestamp { get; set; }
    public string? Description { get; set; }
}

[Serializable]
public class FlathubAppLanguageModel
{
    public required string Value { get; set; }
    public required string Percentage { get; set; }
}

[Serializable]
public class FlathubLaunchableModel
{
    public required string Type { get; set; }
    public required string Value { get; set; }
}

[Serializable]
public class Screenshot
{
    public required List<Size> Sizes { get; set; }
    public string? Caption { get; set; }
    public bool Default { get; set; } // Optional
}

[Serializable]
public class Size
{
    public required string Src { get; set; }
    public required string Scale { get; set; }
    public required string Width { get; set; }
    public required string Height { get; set; }
}

[Serializable]
public class FlathubContentRatingModel
{
    public required string Type { get; set; }
}
