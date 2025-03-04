using BlazeHub.Models.Flathub;
using Gtk;

namespace BlazeHub.Services.Requests;

public static class ScreenshotsRequests
{
    public static async Task<AppScreenshot[]> GetScreenshots(List<Screenshot> screenshots, CancellationToken cancellationToken = default)
    {
        var appImages = new List<AppScreenshot>();
        try
        {
            foreach (var ss in screenshots)
            {
                ss.Sizes.Sort((a, b) => Convert.ToInt32(b.Height).CompareTo(Convert.ToInt32(a.Height)));
                var bestPick = ss.Sizes[0];
                var width = Convert.ToInt32(bestPick.Width);
                var height = Convert.ToInt32(bestPick.Height);
                // if (bestPick == null) continue;

                var pixbuf = await ImageRequests.GetPixbufFromUrl(bestPick.Src, cancellationToken);
                if (pixbuf == null) continue;
                var image = Image.NewFromPixbuf(pixbuf);
                appImages.Add(new AppScreenshot(image, pixbuf, bestPick.Src, width, height, ss.Caption));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return appImages.ToArray();
    }
}