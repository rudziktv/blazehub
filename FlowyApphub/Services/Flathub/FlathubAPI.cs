using System.Net.Http.Json;
using FlowyApphub.Models.Flathub;
using FlowyApphub.Models.FlathubApp;

namespace FlowyApphub.Services.Flathub;

public static class FlathubAPI
{
    private const string FLATHUB_API_URL = "https://flathub.org/api/v2/";
    
    private static readonly HttpClient Client = new();

    static FlathubAPI()
    {
        Client.BaseAddress = new Uri(FLATHUB_API_URL);
    }

    public static async Task<FlathubAppModel?> GetAppDetails(string appId)
    {
        try
        {
            var response = await Client.GetAsync($"appstream/{appId}");
            if (!response.IsSuccessStatusCode) return null;
            // var rawData = await response.Content.ReadAsStringAsync();
            // Console.WriteLine("Raw Data: " + rawData);
            var data = await response.Content.ReadFromJsonAsync<FlathubAppModel>();
            return data;
            // Console.WriteLine(data?.Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public static async Task<FlathubAppSummary?> GetAppSummary(string appId)
    {
        try
        {
            var response = await Client.GetAsync($"summary/{appId}");
            if (!response.IsSuccessStatusCode) return null;
            var data = await response.Content.ReadFromJsonAsync<FlathubAppSummary>();
            return data;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }

    public static async Task<FlathubFullApp?> GetFullApp(string appId)
    {
        try
        {
            var appstream = await GetAppDetails(appId);
            var appSummary = await GetAppSummary(appId);
            if (appSummary == null || appstream == null) return null;
            return new FlathubFullApp(appstream, appSummary);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
    }
}