using Adw;
using FlowyApphub.Utils;
using Gtk;

namespace FlowyApphub.Services.Dialog;

public static class ErrorDialogService
{
    public static void ShowErrorDialog(Exception ex)
    {
        var dialog = Adw.AlertDialog.New(ex.Message, ex.StackTrace);
        dialog.SetCanClose(true);

        // var closeBtn = ButtonUtils.Create("Close");
        // dialog.SetExtraChild(closeBtn);
        //
        // var okBtn = ButtonUtils.Create("Ok");
        // dialog.SetExtraChild(okBtn);
        dialog.AddResponse("close", "Close");
        dialog.AddResponse("copy", "Copy Stack Trace");
        dialog.AddResponse("report", "Report Bug");
        
        dialog.SetResponseAppearance("report", ResponseAppearance.Destructive);

        dialog.OnResponse += (sender, args) =>
        {
            if (args.Response == "report")
            {
                UserDefaultServices.OpenUrl("https://github.com/");
                Console.WriteLine("Reporting bug...");
            }
            else if (args.Response == "copy")
            {
                dialog.GetClipboard().SetText(ex.StackTrace ?? "");
                Console.WriteLine("Copying to clipboard...");
            }
        };

        
        
        dialog.SetDefaultResponse("report");
        dialog.SetCloseResponse("close");
        
        dialog.Present(Program.Application.ActiveWindow);
        
    }
}