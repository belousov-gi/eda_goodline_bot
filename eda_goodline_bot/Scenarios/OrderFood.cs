using System.Security.Cryptography.X509Certificates;

namespace eda_goodline_bot.Scenarios;

public class OrderFood : Scenario
{

    public OrderFood(string scenarioId, List<Step> steps) : base(scenarioId, steps)
    {
        Step.RunStepLogic stepCallback = LogicForSteps;
        RunStepLogic = stepCallback;
        
        // Action.RunActionLogic actionCallback = ff;
        // RunActionLogic = actionCallback;

    }

    private void LogicForSteps(Session session)
    {
        switch (session.CurrentStep.StepId)
        {
            case "currentOrder":
            {
                var answer = OrderManager.ShowOrder(session.UserId);
                session.SocialNetworkAdapter.SendMessage(session.ChatId, answer);
                break;
            }
        }

        
    }
}

