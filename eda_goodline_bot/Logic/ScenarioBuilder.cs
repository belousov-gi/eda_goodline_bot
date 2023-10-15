using System.Text.Json;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public class ScenarioBuilder
{
    public static T CreateScenarioFromJson<T>(string filePath) where T: IScenario
    {
        string jsonString = File.ReadAllText(filePath);
            
        try
        {
            T scenario = JsonSerializer.Deserialize<T>(jsonString) ?? throw new Exception("Scenario is null!");
            return scenario;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}