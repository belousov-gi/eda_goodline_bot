namespace eda_goodline_bot.Iterfaces;

public interface ISocialNetworkAdapter
{
     string ChooseDish();
     void SendGeneralOrder();
     void TestActionAsync(int chatId, string text);

     void Start();

     delegate void OnMessage(int chatId, string text);
     public event OnMessage OnMessages;
}