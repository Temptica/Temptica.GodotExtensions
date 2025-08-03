using Godot;

namespace Temptica.GodotExtensions;

public static class GodotDictionaryExtensions
{
    public static Godot.Collections.Dictionary<string, Variant> Join(this Godot.Collections.Dictionary<string, Variant> dictionary, Godot.Collections.Dictionary<string, Variant> dictionaryToJoin)
    {
        foreach (var key in dictionaryToJoin.Keys)
        {
            dictionary[key] = dictionaryToJoin[key]; // Overwrites if key exists
        }

        return dictionary;
    }
    
}