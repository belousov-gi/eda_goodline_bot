using System.Text.Json;
using eda_goodline_bot.Models;

namespace eda_goodline_bot;

public static class ApplicationConfig
{
    public static string? TokenBotTg { get; private set; }

    public static string? DbStringConnection { get; private set; }
  
    public static void LoadConfigFile(string pathToConfigFile)
    {
        string JsonString = File.ReadAllText(pathToConfigFile);
        var config = JsonSerializer.Deserialize<ApplicationConfigModel>(JsonString);
        TokenBotTg = config?.TokenBotTg;
        DbStringConnection = config?.DbStringConnection;
    }

}