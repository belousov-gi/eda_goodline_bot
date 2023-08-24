namespace eda_goodline_bot.Iterfaces;

public interface IReceivedMessage
{
    public Result[] GeneralMessagesStructure { get; set; }
    
    public class Result
    {
        public Message message { get; set; }
    }
    
    public class Message
    {
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }
    public class From
    {
        public int id { get; set; }
    }
    public class Chat
    {
        public int id { get; set; }
    }

    private void BuildGeneralMessagesStructure(){}
}