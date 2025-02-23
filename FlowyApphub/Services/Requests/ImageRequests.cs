using GdkPixbuf;
using Gtk;

namespace FlowyApphub.Services.Requests;

public static class ImageRequests
{
    public static async Task<Image[]> GetImages(string[] urls, CancellationToken cancellationToken = default)
    {
        var images = new List<Image>();
        try
        {
            foreach (var imageUrl in urls)
            {
                var image = await GetImageFromUrl(imageUrl, cancellationToken);
                if (image == null) continue;
                images.Add(image);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return images.ToArray();
    }
    
    public static async Task<Image?> GetImageFromUrl(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            var pixbuf = await GetPixbufFromUrl(url, cancellationToken);
            return pixbuf is null ? null : Image.NewFromPixbuf(pixbuf);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
    
    public static async Task<Pixbuf?> GetPixbufFromUrl(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new HttpClient();
            var imageBytes = await client.GetByteArrayAsync(url, cancellationToken);
            using var loader = new PixbufLoader();
            loader.Write(imageBytes);
            loader.Close();
            return loader.GetPixbuf();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}