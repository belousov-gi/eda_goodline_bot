namespace eda_goodline_bot;

public class Session
{
    private bool _isExpire;
    private DateTime _dateTimeExpire;
    public Step CurrentStep { get; set; }

    public string UserId { get; private init; }

    public Scenario CurrentScenario { get; set; }
    public string CurrentStepId { get; set; }

    public DateTime DateTimeExpire { get; set; }
    public bool IsExpire
    {
        get
        {
            DateTime currentDate = DateTime.Now;
            
            if (currentDate > DateTimeExpire)
            {
                return true;
            }
            
            DateTimeExpire = currentDate;
            return false;
        }
    }



    public Session(string userId, Scenario currentScenario, Step currentStep, DateTime dateTimeExpire)
    {
        UserId = userId;
        CurrentScenario = currentScenario;
        // CurrentStepId = stepId;
        CurrentStep = currentStep;
    }
    
    public void ActivateStep(string inputText, out string answerText, out string answerMenu)
    {
        answerText = CurrentStep.StepDesc;
        answerMenu = CurrentStep.ActionsList.ToString();
        
        //TODO:спорное решение что это должно быть здесь, возможно стоит вынести
        //в отправку (ведь сообщение может и не отправиться из-за сбоя)

        CurrentStep = CurrentScenario.Steps.Find(step => step.StepId == CurrentStep.NavigateTo);

        // answerText = CurrentStep.ActionsList.Find(action => action.ActionId == inputText).


    }
}