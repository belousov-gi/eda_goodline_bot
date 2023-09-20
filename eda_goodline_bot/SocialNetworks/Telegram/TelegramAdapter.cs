using System.Net;
using eda_goodline_bot.Iterfaces;
using System.Text.Json;
using eda_goodline_bot.Models;

namespace eda_goodline_bot;

//TODO: сделать синглтоном
public class TelegramAdapter : ISocialNetworkAdapter
{
    private HttpClient _httpClient;
    
    private HttpClient telegramClient
    {
        get => _httpClient;
        set
        {
            _httpClient = value;
            _httpClient.Timeout = TimeSpan.FromSeconds(600);
        }
    }
    public string ServerAddress { get; set; }
    public IScenario LoadedScenario { get; init; }
    private JsonSerializerOptions OptionsDisserialization{ get; } =  new() { IncludeFields = true}; 
   
    public event ISocialNetworkAdapter.OnMessage OnMessages;
  
    public TelegramAdapter(string token, IScenario sc)
    {
        telegramClient = new HttpClient();
        ServerAddress = $"https://api.telegram.org/bot{token}";
        LoadedScenario = sc;
    }


    public void Start()
    {

        //смотрим на сообщения, которые пришли до включения бота, чтобы определить с какого updateId начать
        var updateId = FindLastUpdateId(ServerAddress);
        
        //для отладки
        // var updateId = 0;

        ReceiveNewMessages(ServerAddress, updateId);
        
        //TODO: использвать allowed_updates (см доку) . 
    }

    private void ReceiveNewMessages(string serverAddress, int updateId)
    {
        int resultLenght;
        TelegramReceivedMessages messages;
        
        while (true)
        {
            Console.WriteLine($"Ищем новые сообщения, update id {updateId} тред {Thread.GetCurrentProcessorId()}");

            messages = CheckNewMessages(serverAddress, updateId);
            resultLenght = messages.result.Length;
            
            if (resultLenght != 0)
            {
                Console.WriteLine($"Номер треда при триггере события {Thread.GetCurrentProcessorId()}");
                IReceivedMessage messagesGeneralView = messages;
                OnMessages?.Invoke(this, messagesGeneralView);
                updateId = messages.result[resultLenght - 1].update_id; 
                ++updateId;
            }
        }
    }


    private TelegramReceivedMessages CheckNewMessages(string serverAddress, int updateId)
    {
        const int timeout = 60;
        return CheckNewMessages(serverAddress, updateId, timeout);
    }
    private TelegramReceivedMessages CheckNewMessages(string serverAddress, int updateId, int timeout)
    {
        string requestStr = serverAddress + $"/getUpdates?timeout={timeout}&offset={updateId}";
        using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);
        using var response = telegramClient.Send(request);
        string responseText = response.Content.ReadAsStringAsync().Result;
        TelegramReceivedMessages messages = JsonSerializer.Deserialize<TelegramReceivedMessages>(responseText, OptionsDisserialization);
        messages.BuildGeneralMessagesStructure();
        return messages;
    }
    
    
    private int FindLastUpdateId(string serverAddress)
    {
        int updateId = 0;
        TelegramReceivedMessages? messages = CheckNewMessages(serverAddress, updateId, 0);
        
        Console.WriteLine($"смотрим кол-во пропущенных сообщений {messages.result.Length}");
        int resultLenght = messages.result.Length;
        
        if (resultLenght != 0)
        {
            updateId = messages.result[resultLenght - 1].update_id;
            updateId++;
        }
        return updateId;
    }

    private void SendMessageToTgApi(string requestStr)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);

        // Console.WriteLine($"Номер треда внутри хендлера ДО отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");

        using var response = telegramClient.Send(request);
                
        if (response.StatusCode != HttpStatusCode.OK)
        {
            //TODO:залогировать ошибку отправки
        }
    }

    public void SendMessage(int chatId, string answerText, List<Action> actionsList)
    {
        string answerMenu;
        string answerMenuJSON = "";

        
        foreach (var action in actionsList)
        {
            answerMenu = JsonSerializer.Serialize(action.button) + ",";
            answerMenuJSON += answerMenu;
        }
        // string answerMenuJSON = JsonSerializer.Serialize(answerMenu);
        answerMenuJSON = answerMenuJSON.Remove(answerMenuJSON.Length - 1);
        string jsonAnswer = "{\"keyboard\":[" + answerMenuJSON+ "]}";

        var requestStr =
            ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{answerText}&reply_markup={jsonAnswer}"}";
        SendMessageToTgApi(requestStr);
    }
    public void SendMessage(int chatId, string answerText)
    {
        var requestStr = ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{answerText}"}";
        SendMessageToTgApi(requestStr);
    }
}
