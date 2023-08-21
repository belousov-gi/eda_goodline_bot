using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public class Session
{
    private bool _isExpire;
    private DateTime _dateTimeExpire;
    public string UserId { get; private init; }
    public int ChatId { get; init; }
    public ISocialNetworkAdapter SocialNetworkAdapter { get; init; }
    public Scenario CurrentScenario { get; set; }

    public Step CurrentStep { get; set; }

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



    public Session(ISocialNetworkAdapter socialNetworkAdapter, string userId, int chatId, Scenario currentScenario, Step? currentStep, DateTime dateTimeExpire)
    {
        SocialNetworkAdapter = socialNetworkAdapter;
        UserId = userId;
        ChatId = chatId;
        CurrentScenario = currentScenario;
        CurrentStep = currentStep;
        DateTimeExpire = dateTimeExpire;
    }

}