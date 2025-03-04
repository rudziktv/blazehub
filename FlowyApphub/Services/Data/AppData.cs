namespace BlazeHub.Services.Data;

public static class AppData
{
    public static readonly string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
    public static readonly string TempDataFolder = Path.Combine(Path.GetTempPath(), AppInfo.APP_ID);
    
    static AppData()
    {
        CreateTempDataFolder();
    }

    private static void CreateTempDataFolder()
    {
        // Cache
        
        
        Console.WriteLine("SpecialPath -> " + Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
        
        try
        {
            if (!Directory.Exists(TempDataFolder))
                Directory.CreateDirectory(TempDataFolder);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}