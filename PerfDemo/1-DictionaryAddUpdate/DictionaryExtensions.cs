using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace PerfDemo._1_DictionaryAddUpdate;

public static class DictionaryExtensions
{
    public static TValue? GetOrAdd<TKey, TValue>
        (this Dictionary<TKey, TValue> dict, TKey key, TValue? value)
        where TKey : notnull
    {
        if (dict.TryGetValue(key, out var existingValue))
            return existingValue;
        
        dict[key] = value;

        return value;
    }

    public static TValue? GetOrAddFast<TKey, TValue>
        (this Dictionary<TKey, TValue> dict, TKey key, TValue? value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out bool exists); // Show IL Spy

        if (exists)
            return val;

        val = value;
        return value;
    }

    public static bool TryUpdate<TKey, TValue>
        (this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        if (!dict.ContainsKey(key))
            return false;
        
        dict[key] = value;

        return true;
    }

    public static bool TryUpdateFast<TKey, TValue>
        (this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        where TKey : notnull
    {
        ref var val = ref CollectionsMarshal.GetValueRefOrNullRef(dict, key);
        
        if (Unsafe.IsNullRef(ref val))
            return false;

        val = value;
        return true;
    }
}
