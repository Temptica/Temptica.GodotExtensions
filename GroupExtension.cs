using System.Text.RegularExpressions;
using Godot;
using Temptica.GodotExtensions.Models;

namespace Temptica.GodotExtensions;

public static class GroupExtension
{
    /// <summary>
    /// easier way to call group methods using statically typed strings
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="methode"></param>
    /// <param name="variants"></param>
    /// <typeparam name="T"></typeparam>
    public static void CallGroup<T>(this SceneTree tree, StringName methode, params Variant[] variants) where T : IGroup
    {
        var groupName = typeof(T).Name[..^5];
        tree.CallGroup(groupName, methode, variants);
    }

    public static void CallGroup<T>(this Node node, StringName methode, params Variant[] variants) where T : IGroup
    {
        node.GetTree().CallGroup<T>(methode, variants);
        
    }

    public static void AddToGroup<T>(this Node node) where T : IGroup
    {
        var groupName = typeof(T).Name[..^5];
        node.AddToGroup(groupName);
    }
}