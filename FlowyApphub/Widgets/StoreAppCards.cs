using BlazeHub.Models.Flathub;
using BlazeHub.Services.Flathub.Safety;
using Gtk;

namespace BlazeHub.Widgets;

public static class StoreAppCards
{
    public static Widget GetLicenseCard(FlathubAppModel appstream)
    {
        if (!appstream.IsFreeLicense)
            return GetProprietaryLicenseCard();
        return GetCommunityLicenseCard(appstream);
    }

    private static CardWithBadge GetCommunityLicenseCard(FlathubAppModel appstream)
    {
        var card = new CardWithBadge();
        var heartBadge = new Badge("emote-love-symbolic");
        var communityBadge = new Badge("org.gnome.Settings-users-symbolic");
        var eyeBadge = new Badge("view-reveal-symbolic");
        card.AppendBadges(heartBadge);
        card.AppendBadges(communityBadge);
        card.AppendBadges(eyeBadge);
        
        var typeLabel = Label.New("Community Built");
        typeLabel.AddCssClass("heading");
        card.AppendLabel(typeLabel);
        card.AppendLabel(GetCommunityLicenseLabel(appstream.ProjectLicense));
        
        return card;
    }

    private static Label GetCommunityLicenseLabel(string license)
    {
        var url = GetLicenseUrl(license);
        var text =
            $"This app is developed in the open by international community and released under the <a href='{url}'><b>{license}</b></a>.";
        
        var advise = Label.New(text);
        advise.SetUseMarkup(true);
        advise.SetHexpand(true);
        advise.SetWrap(true);
        advise.SetValign(Align.Center);
        advise.SetJustify(Justification.Center);
        return advise;
    }

    private static string GetLicenseUrl(string license)
    {
        if (license == "GPL-3.0-or-later" || license == "GPL-3.0")
            return "https://choosealicense.com/licenses/gpl-3.0/";
        if (license == "Apache-2.0")
            return "https://choosealicense.com/licenses/apache-2.0/";
        if (license == "GPL-2.0")
            return "https://choosealicense.com/licenses/gpl-2.0/";
        if (license == "MPL-2.0")
            return "https://choosealicense.com/licenses/mpl-2.0/";
        if (license == "MIT")
            return "https://choosealicense.com/licenses/mit/";
        if (license is "AGPL-3.0-or-later" or "AGPL-3.0")
            return "https://choosealicense.com/licenses/agpl-3.0/";
        
        return "https://choosealicense.com/licenses/";
    }
    
    private static CardWithBadge GetProprietaryLicenseCard()
    {
        var card = new CardWithBadge();
        
        var noPrivacyBadge = new Badge("preferences-system-privacy-symbolic", "warning");
        var warningBadge = new Badge("dialog-warning-symbolic", "warning");
        var noEyeBadge = new Badge("view-conceal-symbolic", "warning");
        card.AppendBadges(noPrivacyBadge);
        card.AppendBadges(warningBadge);
        card.AppendBadges(noEyeBadge);
        
        var typeLabel = Label.New("Proprietary");
        typeLabel.AddCssClass("heading");
        card.AppendLabel(typeLabel);

        var licenseAdvise = Label.New($"This app is not developed in the open, so only its developers know how it works. It may be insecure in ways that are hard to detect, and it may change without oversight..");
        licenseAdvise.SetHexpand(true);
        licenseAdvise.SetWrap(true);
        card.AppendLabel(licenseAdvise);
        
        return card;
    }

    public static CardWithBadge GetSafetyCard(FlathubAppPermissions permissions)
    {
        var card = new CardWithBadge();
        var safetyFeatures = FlathubSafety.GetAppSafetyFeatures(permissions);
        var safetyScore = FlathubSafety.GetAppSafetyScore(safetyFeatures);

        var safeBadge = new Badge("channel-secure-symbolic");
        var unsafeBadge = new Badge("dialog-question-symbolic", "error");
        var probablySafeBadge = new Badge("emblem-default-symbolic", "warning");
        var safetyLabel = Label.New("Probably Safe");
        
        switch (safetyScore)
        {
            case 0:
                card.AppendBadges(unsafeBadge);
                safetyLabel.SetText("Potentially unsafe");
                break;
            case 1:
                card.AppendBadges(probablySafeBadge);
                break;
            default:
                card.AppendBadges(safeBadge);
                safetyLabel.SetText("Safe");
                break;
        }
        
        safetyLabel.AddCssClass("heading");
        card.AppendLabel(safetyLabel);
        card.AppendLabel(GetShortPermissions(safetyFeatures));
        
        return card;
    }

    private static Widget GetShortPermissions(List<IFlathubSafetyFeature> features)
    {
        var box = Box.New(Orientation.Vertical, 0);
        var shortFeatures = FlathubSafety.GetAppSafetyShortFeatures(features);

        foreach (var feature in shortFeatures)
        {
            box.Append(Label.New(feature));
        }

        return box;
    }
}