namespace BlazeHub.Utils;

public static class DateUtils
{
    public static string TimeAgo(DateTime dateTime)
    {
        var ts = DateTime.Now.Subtract(dateTime);
        
        if (ts.TotalSeconds < 60) return "a moment ago";
        if (ts.TotalMinutes < 60) return $"{(int)ts.TotalMinutes} minutes ago";
        if (ts.TotalHours < 24) return $"{(int)ts.TotalHours} {((int)ts.TotalHours == 1 ? "hour" : "hours")} ago";
        if (ts.TotalDays < 30) return $"{(int)ts.TotalDays} {((int)ts.TotalDays == 1 ? "day" : "days")} ago";
        if (ts.TotalDays < 365) return $"{(int)(ts.TotalDays / 30f)} {((int)(ts.TotalDays / 30) == 1 ? "month" : "months")} ago";
        
        return $"{(int)(ts.TotalDays / 365)} {((int)(ts.TotalDays / 365) == 1 ? "year" : "years")} ago";
    }
}