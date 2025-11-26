using Godot;

namespace Temptica.GodotExtensions;

public static class NodeExtensions
{
    public static List<T> GetAllChildren<T>(this Node? node) where T : GodotObject
    {
        if(node == null) return [];
        var children = node.GetChildren().ToList();
        var childrenOfT = children.OfType<T>().ToList();

        foreach (var childNode in children)
        {
            childrenOfT.AddRange(childNode.GetAllChildren<T>());
        }
        return childrenOfT;
    }
    
    public static bool HasChild(this Node node, Node child) => node.GetChildren().Any(n => n == child);
    public static void RemoveChildIfExists(this Node node, Node child)
    {
        if (!node.HasChild(child)) return;
        node.RemoveChild(child);
    }
}