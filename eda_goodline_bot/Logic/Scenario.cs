namespace eda_goodline_bot;

//сценарий, который состоит из шагов (экранов с действиями)
public class Scenario
{
    public string ScenarioId { get; private init; }
    public Step[] StepsList { get; private init; }

    public Scenario(string scenarioId, Step[] stepsList)
    {
        ScenarioId = scenarioId;
        StepsList = stepsList;
    }

}