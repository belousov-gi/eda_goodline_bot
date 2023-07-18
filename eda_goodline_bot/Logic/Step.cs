namespace eda_goodline_bot;

//один экран, который содержит какие-то варианты действий
public class Step
{
    public string StepId { get; private init;}
    public List<Action> ActionsList { get; private init; }

    public Step(string stepId, List<Action> actionsList)
    {
        StepId = stepId;
        ActionsList = actionsList;
    }
}