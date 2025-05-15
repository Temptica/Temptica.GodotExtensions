using Godot;

namespace Temptica.GodotExtensions;

public static class Node2DExtensions
{
    public static async Task QueueRedrawAsync(this CanvasItem node)
    {
        await node.CallAsync(CanvasItem.MethodName.QueueRedraw);
    }
    
}