using Telegram.Bot.Types.ReplyMarkups;

namespace eda_goodline_bot;

//один экран, который содержит какие-то варианты действий
public class Step
{
    public string StepId { get; init;}
    public string StepDesc { get;  init;}
    
    public Action? LastAction { get;  set; }
    public List<Action> Actions { get;  set; }
    public RunStepLogic? StepLogic { get; set; }
    
    // public List<List<KeyboardButton>> ActionsList { get; private init; }

    public Step(string stepId, string stepDesc, List<Action> actions)
    {
        StepId = stepId;
        Actions = actions;
        StepDesc = stepDesc;
    }
    
    public delegate void RunStepLogic(Session session);

}