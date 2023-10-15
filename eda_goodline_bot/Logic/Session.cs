using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

public class Session
{
    private bool _isExpire;
    private DateTime _dateTimeExpire;
    public int UserId { get; private init; }
    public int ChatId { get;}
    public ISocialNetworkAdapter SocialNetworkAdapter { get;}
    public IScenario CurrentScenario { get;}
    public Step CurrentStep { get; set; }

    public Session(ISocialNetworkAdapter socialNetworkAdapter, int userId, int chatId, IScenario currentScenario, Step? currentStep)
    {
        SocialNetworkAdapter = socialNetworkAdapter;
        UserId = userId;
        ChatId = chatId;
        CurrentScenario = currentScenario;
        CurrentStep = currentStep;
    }

}