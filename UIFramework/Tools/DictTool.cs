using System.Collections.Generic;

public static class DictTool
{
    public static Tvalue GetValue<TKey, Tvalue>(this Dictionary<TKey, Tvalue> dict, TKey key)
    {
        Tvalue value = default;
        dict.TryGetValue(key, out value);
        return value;
    }
}