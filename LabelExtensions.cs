using System.Diagnostics.CodeAnalysis;
using Godot;

namespace Temptica.GodotExtensions;

public static class LabelExtensions
{
    public static void SetTranslatedText(this Label label, string text)
    {
        label.Text = TranslationServer.Translate(text);
        if (label.Text == text)
        {
            label.Text = $"NO TRANSLATION: {text}";
        }
    }
}