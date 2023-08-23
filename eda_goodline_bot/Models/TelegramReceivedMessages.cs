using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot.Models;

public class TelegramReceivedMessages : IReceivedMessage
{
    public IReceivedMessage.Result[] result { get; set; }
    
}