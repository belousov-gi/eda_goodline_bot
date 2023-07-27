using Telegram.Bot.Types.ReplyMarkups;

namespace eda_goodline_bot;

//один экран, который содержит какие-то варианты действий
public class Step
{
    public string StepId { get; private init;}
    public string NavigateTo { get; private init;}
    public string StepDesc { get; private init;}

    public List<Action> ActionsList { get; private init; }
    
    // public List<List<KeyboardButton>> ActionsList { get; private init; }

    public Step(string stepId, string stepDesc, List<Action> actionsList, string navigateTo)
    {
        StepId = stepId;
        ActionsList = actionsList;
        StepDesc = stepDesc;
        NavigateTo = navigateTo;
    }

}