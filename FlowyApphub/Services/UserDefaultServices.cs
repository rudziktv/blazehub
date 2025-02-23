using System.Diagnostics;
using FlowyApphub.Services.Dialog;

namespace FlowyApphub.Services;

public static class UserDefaultServices
{
    public static void OpenUrl(string url)
    {
        try
        {
            Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
    }
}