using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot
{
    public class TelegramConnection : ISocialNetworkAdapter
    {
        public string ChooseDish()
        {
            return "your order";
        }

        public void SendGeneralOrder()
        {
            throw new NotImplementedException();
        }
    } 
}

