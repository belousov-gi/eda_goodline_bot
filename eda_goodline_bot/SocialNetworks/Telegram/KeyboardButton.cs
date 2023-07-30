using System.Text.Json.Serialization;

namespace eda_goodline_bot;

public class KeyboardButton
{
    public string Text { get; set; }

    public KeyboardButton(string text)
    {
        Text = text;
    }
}