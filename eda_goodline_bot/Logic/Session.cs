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



    public Session(string userId, Scenario currentScenario, Step? currentStep, DateTime dateTimeExpire)
    {
        UserId = userId;
        CurrentScenario = currentScenario;
        CurrentStep = currentStep;
        DateTimeExpire = dateTimeExpire;
    }

}