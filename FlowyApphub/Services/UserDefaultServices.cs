using System.Diagnostics;
using BlazeHub.Services.Dialog;

namespace BlazeHub.Services;

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