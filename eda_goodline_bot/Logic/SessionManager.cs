using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

static class SessionManager
{
    private const int _timeForExpire = 24;
    private const string _firstStepId = "/start";
    
    public static List<Session> SessionsList = new List<Session>();

    public static Session CreateSession(ISocialNetworkAdapter socialNetworkAdapter, string userId, int chatId, Scenario currentScenario)
    {
        try
        {
            DateTime dateTimeExpire = DateTime.Now.AddHours(_timeForExpire);
            Step currentStep = currentScenario.Steps.Find(step => step.StepId == _firstStepId) ?? throw new InvalidOperationException("/start hasn't been found");
            Session newSession = new Session( socialNetworkAdapter, userId, chatId, currentScenario, currentStep, dateTimeExpire);
            SessionsList.Add(newSession);
            return newSession;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("EXCEPTION");
        }

    }
    
   

}