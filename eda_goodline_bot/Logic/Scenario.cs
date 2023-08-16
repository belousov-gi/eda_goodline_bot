namespace eda_goodline_bot;

//TODO: сделать синглтоном
//сценарий, который состоит из шагов (экранов с действиями)
public class Scenario
{
    public Step.RunStepLogic RunStepLogic { get; init; }
    public Action.RunActionLogic RunActionLogic { get; init; }
    public string ScenarioId { get; set; }
    public List<Step> Steps { get; set; }
    

    public Scenario(string scenarioId, List<Step> steps)
    {
        ScenarioId = scenarioId;
        Steps = steps;
    }
    
    

}