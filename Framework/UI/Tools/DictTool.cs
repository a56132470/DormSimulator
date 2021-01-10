using System.Collections.Generic;

public static class DictTool
{
    public static Tvalue GetValue<TKey, Tvalue>(this Dictionary<TKey, Tvalue> dict, TKey key)
    {
        dict.TryGetValue(key, out var value);
        return value;
    }
}