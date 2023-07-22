namespace eda_goodline_bot.Iterfaces;

public interface ISocialNetworkAdapter
{
     void SendGeneralOrder();
     void MessageHandler(int chatId, string text);

     void Start();

     delegate void OnMessage(int chatId, string text);
     public event OnMessage OnMessages;
}