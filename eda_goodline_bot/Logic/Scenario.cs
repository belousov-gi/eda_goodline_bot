namespace eda_goodline_bot;

//TODO: сделать синглтоном
//сценарий, который состоит из шагов (экранов с действиями)
public class Scenario
{
    public string ScenarioId { get; set; }
    public List<Step> Steps { get; set; }

    public Scenario(string scenarioId, List<Step> stepsList)
    {
        ScenarioId = scenarioId;
        Steps = stepsList;
    }
    
    

}