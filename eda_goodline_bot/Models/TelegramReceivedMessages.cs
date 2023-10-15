using System.Text.Json.Serialization;
using eda_goodline_bot.Iterfaces;

namespace eda_goodline_bot.Models;

public class TelegramReceivedMessages : IReceivedMessage
{

    public IReceivedMessage.Result[] GeneralMessagesStructure { get; set; }

    [JsonInclude]
    public bool ok { get; set; }
    
    [JsonInclude]
    public Result[] result { get; set; }
    
    public class Result
    {
        public int update_id { get; set; }
        public Message message { get; set; }
    }

    public class Message
    {
        public int message_id { get; set; }
        public From from { get; set; }
        public Chat chat { get; set; }
        public int date { get; set; }
        public string text { get; set; }
    }

    public class From
    {
        public int id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string language_code { get; set; }
    }

    public class Chat
    {
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string username { get; set; }
        public string type { get; set; }
    }

    public void BuildGeneralMessagesStructure()
    {
        int i = 0;
        GeneralMessagesStructure = new IReceivedMessage.Result[result.Length];
        
        foreach (var res in result)
        {
            GeneralMessagesStructure[i] = new IReceivedMessage.Result();
            GeneralMessagesStructure[i].message = new IReceivedMessage.Message();
            GeneralMessagesStructure[i].message.from = new IReceivedMessage.From();
            GeneralMessagesStructure[i].message.chat = new IReceivedMessage.Chat();
            GeneralMessagesStructure[i].message.from.id = res.message.from.id;
            GeneralMessagesStructure[i].message.from.username = res.message.from.username;
            GeneralMessagesStructure[i].message.chat.id = res.message.chat.id;
            GeneralMessagesStructure[i].message.date = res.message.date;
            GeneralMessagesStructure[i].message.text = res.message.text;
            i++;
        }
        
    }


}

























