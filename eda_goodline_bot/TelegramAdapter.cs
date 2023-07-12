using eda_goodline_bot.Iterfaces;
using System.Net.Http;


namespace eda_goodline_bot;

public class TelegramAdapter : ISocialNetworkAdapter
{
    
    // TODO: понять как реализовать HTTP клент с учетом его времени жизни (DNS могут меняться) 
    public string ChooseDish()
    {
        throw new NotImplementedException();
    }

    public void SendGeneralOrder()
    {
        throw new NotImplementedException();
    }
    
    
}