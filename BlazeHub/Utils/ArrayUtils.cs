namespace BlazeHub.Utils;

public static class ArrayUtils
{
    public static bool Contains<TSource>(this IEnumerable<TSource> array, Func<TSource, bool> predicate)
        => array.FirstOrDefault(predicate) != null;
    
}