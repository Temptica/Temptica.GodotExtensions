using Godot;

namespace Temptica.GodotExtensions;

public static class NodeExtensions
{
    public static List<T> GetAllChildren<T>(this Node node) where T : GodotObject
    {
        var children = node.GetChildren().ToList();
        var childrenOfT = children.OfType<T>().ToList();

        foreach (var childNode in children)
        {
            childrenOfT.AddRange(childNode.GetAllChildren<T>());
        }
        return childrenOfT;
    }
}