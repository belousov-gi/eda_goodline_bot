using Telegram.Bot.Types.ReplyMarkups;

namespace eda_goodline_bot;

//один экран, который содержит какие-то варианты действий
public class Step
{
    public string StepId { get; private init;}

    public List<List<KeyboardButton>> ActionsList { get; private init; }

    public Step(string stepId, List<List<KeyboardButton>> actionsList)
    {
        StepId = stepId;
        ActionsList = actionsList;
    }

    public void UseStep(string inputAction)
    {
        
    }

}