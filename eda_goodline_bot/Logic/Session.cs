namespace eda_goodline_bot;

public class Session
{
    private bool _isExpire;
    private DateTime _dateTimeExpire;
    public string _stepId;

    public string UserId { get; private init; }

    public Scenario CurrentScenario { get; set; }
    public string StepId
    {
        get => _stepId;
        set
        {
            if (_stepId == null)
            {
                //TODO: доделать дефолтный шаг
            }
        }
    }

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



    public Session(string userId, Scenario currentScenario, string stepId, DateTime dateTimeExpire)
    {
        UserId = userId;
        CurrentScenario = currentScenario;
        StepId = stepId;
    }

}