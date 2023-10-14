using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot;

static class SessionManager
{
    private const string _firstStepId = "/start";
    
    public static List<Session> SessionsList = new();

    public static Session CreateSession(ISocialNetworkAdapter socialNetworkAdapter, int userId, int chatId, IScenario currentScenario)
    {
        try
        {
            Step currentStep = currentScenario.Steps.Find(step => step.StepId == _firstStepId) ?? throw new InvalidOperationException("/start hasn't been found");
            Session newSession = new Session( socialNetworkAdapter, userId, chatId, currentScenario, currentStep);
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