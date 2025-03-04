using System.Diagnostics;
using BlazeHub.Services.Dialog;
using GdkPixbuf;

namespace BlazeHub.Services.Images;

public static class ImagesService
{
    public static bool SaveImageFromUrl(Pixbuf pixbuf, string url, string directory, out string path)
    {
        var uri = new Uri(url);
        return SaveImageFromUrl(pixbuf, uri, directory, out path);
    }
    
    public static bool SaveImageFromUrl(Pixbuf pixbuf, Uri uri, string directory, out string path)
    {
        var filename = Path.GetFileName(uri.LocalPath);
        path = Path.Combine(directory, filename);

        try
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            return SaveImage(pixbuf, path);
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
        
        return false;
    }
    
    public static bool SaveImage(Pixbuf pixbuf, string path)
    {
        var directory = Path.GetDirectoryName(path);
        var extension = Path.GetExtension(path);
        
        if (string.IsNullOrEmpty(directory))
            return false;

        try
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            
            return SaveImage(pixbuf, path, extension);
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
        
        return false;
    }

    /// <summary>
    /// Saves pixbuf to desired path. It doesn't create path!
    /// </summary>
    /// <param name="pixbuf"></param>
    /// <param name="path">Path to final file</param>
    /// <param name="extension">Extension without '.'</param>
    /// <returns>If saving operation succeed</returns>
    public static bool SaveImage(Pixbuf pixbuf, string path, string extension)
    {
        try
        {
            return pixbuf.Savev(path, extension.Replace(".", ""), null, null);
        }
        catch (Exception e)
        {
            ErrorDialogService.ShowErrorDialog(e);
            Console.WriteLine(e);
        }
        return false;
    }

    public static bool OpenImageViewer(string path)
    {
        try
        {
            Process.Start("xdg-open", path);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return false;
    }
}