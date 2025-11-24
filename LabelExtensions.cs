using System.Diagnostics.CodeAnalysis;
using Godot;

namespace Temptica.GodotExtensions;

public static class LabelExtensions
{
    public static void SetTranslatedText(this Label label, string text, params object?[] args)
    {
        label.Text = TranslationServer.Translate(text);
        if (label.Text == text)
        {
            label.Text = $"NO TRANSLATION: {text}";
            return;
        }

        if (args.Length > 0) label.Text = string.Format(label.Text, args);
    }
    
    public static void SetTranslatedText(this Label3D label, string text, params object?[] args)
    {
        label.Text = TranslationServer.Translate(text);
        if (label.Text == text)
        {
            label.Text = $"NO TRANSLATION: {text}";
            return;
        }

        if (args.Length > 0) label.Text = string.Format(label.Text, args);
    }

    public static void SetTranslatedText(this RichTextLabel label, string text, params object?[] args)
    {
        label.Text = TranslationServer.Translate(text);
        if (label.Text == text)
        {
            label.Text = $"NO TRANSLATION: {text}";
            return;
        }

        if (args.Length > 0) label.Text = string.Format(label.Text, args);
    }
}