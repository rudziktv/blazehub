using System.Globalization;
using System.Text.RegularExpressions;

namespace FlowyApphub.Utils;

public static partial class NetworkUnitUtils
{
    public static uint SpeedToBytes(string input)
    {
        var regex = SpeedNetworkRegex();
        var match = regex.Match(input);
        if (!match.Success) return 0;
        
        var value = float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var unit = match.Groups[2].Value;
        
        var multiplier = unit switch
        {
            "bytes/s" => 1u,
            "kB/s" => 1024u,
            "MB/s" => 1024u * 1024u,
            "GB/s" => 1024u * 1024u * 1024u,
            _ => 1u
        };
        return (uint)Math.Round(value * multiplier);
    }

    public static uint SizeToBytes(string input)
    {
        var regex = DataSizeRegex();
        var match = regex.Match(input);
        if (!match.Success) return 0;
        
        var value = float.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
        var unit = match.Groups[2].Value;
        
        var multiplier = unit switch
        {
            "bytes" => 1u,
            "kB" => 1024u,
            "MB" => 1024u * 1024u,
            "GB" => 1024u * 1024u * 1024u,
            _ => 1u
        };
        return (uint)Math.Round(value * multiplier);
    }

    public static string BytesToString(ulong bytes)
    {
        var size = BytesToOptimal(bytes, out var unit);
        var alias = DataUnitsToAlias(unit);
        
        return unit switch
        {
            DataUnits.Bytes => $"{size:F0} {alias}",
            DataUnits.KiloBytes => $"{size:F0} {alias}",
            DataUnits.MegaBytes => $"{size:F2} {alias}",
            DataUnits.GigaBytes => $"{size:F2} {alias}",
            _ => $"{size:F2} Unknown"
        };
    }

    public static double BytesToOptimal(ulong bytes, out DataUnits units)
    {
        double result = bytes;
        units = DataUnits.Bytes;
        while (result > 1024 && units < DataUnits.GigaBytes)
        {
            result /= 1024;
            units += 1;
        }
        return result;
    }

    public static double BytesToUnits(ulong bytes, DataUnits units)
    {
        if (units == 0)
            return bytes;
        return BytesToUnits(bytes, units - 1) / 1024;
    }

    public static string DataUnitsToAlias(DataUnits units) => units switch
    {
        DataUnits.Bytes => "bytes",
        DataUnits.KiloBytes => "kB",
        DataUnits.MegaBytes => "MB",
        DataUnits.GigaBytes => "GB",
        _ => "bytes"
    };

    [GeneratedRegex(@"([\d\.,]+).(\w+\/s)")]
    private static partial Regex SpeedNetworkRegex();
    [GeneratedRegex(@"([\d\.,]+).(\w+)")]
    private static partial Regex DataSizeRegex();
}