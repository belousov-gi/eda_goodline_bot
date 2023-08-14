using eda_goodline_bot.Models;

namespace eda_goodline_bot.Iterfaces;

public interface ISocialNetworkAdapter
{
     void SendGeneralOrder();

     void Start();
     void SendMessage(int chatId, string answerText, List<Action> actionsList);
     void SendMessage(int chatId, string answerText);
     public Scenario LoadedScenario { get; init; }
     
     public delegate void OnMessage(ISocialNetworkAdapter socialNetworkAdapter, TelegramReceivedMessages messages);
     public event OnMessage OnMessages;


}