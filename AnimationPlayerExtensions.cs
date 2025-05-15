using Godot;

namespace Temptica.GodotExtensions;

public static class AnimationPlayerExtensions
{
    public static async Task PlayAsync(this AnimationPlayer animationPlayer, string? animation = null)
    {
        if (animation == null)
        {
            await animationPlayer.CallAsync(AnimationPlayer.MethodName.Play);
            return;
        }
        await animationPlayer.CallAsync(AnimationPlayer.MethodName.Play, animation);
    }
}