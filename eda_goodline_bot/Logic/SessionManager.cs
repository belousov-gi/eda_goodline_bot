namespace eda_goodline_bot;

static class SessionManager
{
    private const int _timeForExpire = 24;
    private const string _firstStepId = "/start";
    
    public static List<Session> SessionsList = new List<Session>();

    public static Session CreateSession(string userId, Scenario currentScenario)
    {
        DateTime dateTimeExpire = DateTime.Now.AddHours(_timeForExpire);
        var firstStep = currentScenario.Steps.Find(step => step.StepId == _firstStepId);
        Session newSession = new Session(userId, currentScenario, firstStep, dateTimeExpire);
        SessionsList.Add(newSession);
        return newSession;
    }
    
   

}