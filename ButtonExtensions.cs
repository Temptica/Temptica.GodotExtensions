using Godot;

namespace Temptica.GodotExtensions;

public static class ButtonExtensions
{
    public static void SetTranslatedText(this Button label, string text, params object?[] args)
    {
        label.Text = TranslationServer.Translate(text);
        if (args.Length > 0) label.Text = string.Format(label.Text, args);
    }
}