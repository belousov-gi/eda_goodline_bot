using eda_goodline_bot.Models;

namespace eda_goodline_bot.Iterfaces;

public interface ISocialNetworkAdapter
{
     void Start();
     void SendMessage(int chatId, string answerText, List<Action> actionsList);
     void SendMessage(int chatId, string answerText);
     void HandleUserInfoDbAsync(string nickNameTg, int userId, int chatId);
     
     public IScenario LoadedScenario { get; init; }
     
     public delegate void OnMessage(ISocialNetworkAdapter socialNetworkAdapter, IReceivedMessage messages);
     public event OnMessage OnMessages;


}