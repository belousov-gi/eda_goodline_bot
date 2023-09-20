namespace eda_goodline_bot.Iterfaces;

public interface IScenario
{
    string ScenarioId { get; set; }
    List<Step> Steps { get; set; }

    void AddLogicToScenario();
}