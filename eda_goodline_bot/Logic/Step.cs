using Telegram.Bot.Types.ReplyMarkups;

namespace eda_goodline_bot;

//один экран, который содержит какие-то варианты действий
public class Step
{
    public string StepId { get; init;}
    public string NavigateTo { get;  init;}
    public string StepDesc { get;  init;}

    public List<Action> Actions { get;  init; }
    
    // public List<List<KeyboardButton>> ActionsList { get; private init; }

    public Step(string stepId, string stepDesc, List<Action> actions, string navigateTo)
    {
        StepId = stepId;
        Actions = actions;
        StepDesc = stepDesc;
        NavigateTo = navigateTo;
    }

}