namespace eda_goodline_bot;

public class Session
{
    private bool _isExpire;
    private DateTime _dateTimeExpire;
    public int TimeForExpire{get; private init; }

    public string UserId { get; private init; }
    public string ChatId { get; private init; }
    
    public string ScenarioId { get; set; }
    public string StepId { get; set; }

    public DateTime DateTimeExpire
    {
        get => _dateTimeExpire;
        set
        {
            _dateTimeExpire = value.AddHours(TimeForExpire);
        }
    }

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

    public Session(string userId, string chatId, string scenarioId, string stepId)
    {
        UserId = userId;
        ChatId = chatId;
        ScenarioId = scenarioId;
        StepId = stepId;
        TimeForExpire = 24;
        DateTimeExpire = DateTime.Now;
    }

}