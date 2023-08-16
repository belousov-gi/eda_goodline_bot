using System.Net;
using eda_goodline_bot.Iterfaces;
using System.Net.Http;
using System.Reflection;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.Json;
using System.Text.Json.Nodes;
using eda_goodline_bot.Models;
using static eda_goodline_bot.Program;


namespace eda_goodline_bot;

public class TelegramAdapter : ISocialNetworkAdapter
{
    private HttpClient _httpClient;
    
    private HttpClient telegramClient
    {
        get => _httpClient;
        set
        {
            _httpClient = value;
            _httpClient.Timeout = TimeSpan.FromSeconds(60);
        }
    }

    // TODO: УДАЛИТь и из конструктора тоже!!
    private readonly string token;
    public string ServerAddress { get; set; }
    public Scenario LoadedScenario { get; init; }
    
    public delegate void OnMessage(ISocialNetworkAdapter socialNetworkAdapter, TelegramReceivedMessages messages);
    public event ISocialNetworkAdapter.OnMessage OnMessages;

    // public TelegramAdapter(string token, string fileName)
    // {
    //     telegramClient = new HttpClient();
    //     this.token = token;
    //     ServerAddress = $"https://api.telegram.org/bot{token}";
    //     LoadedScenario = CreateScenarioFromJson(fileName);
    //
    // }
    
    public TelegramAdapter(string token, Scenario sc)
    {
        telegramClient = new HttpClient();
        this.token = token;
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
        
        //TODO: Прием сообщений куда-то в отдельный хендлер надо вынести + использвать allowed_updates (см доку) . 

        
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
                OnMessages?.Invoke(this, messages);
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
        var messages = JsonSerializer.Deserialize<TelegramReceivedMessages>(responseText);
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

    public void SendGeneralOrder()
    {
        throw new NotImplementedException();
    }

    public void SendMessage(int chatId, string answerText, List<Action> actionsList)
    {
        string answerMenu = "";
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



   
    
    // public static void ActivateStep(Session userSession,string inputText, out string answerText, out string answerMenu)
    // {
    //     //только при заходе на шаг
    //     answerText = CurrentStep.StepDesc;
    //     answerMenu = CurrentStep.Actions.ToString();
    //       
    //     
    //     //TODO:спорное решение что это должно быть здесь, возможно стоит вынести
    //     //в отправку (ведь сообщение может и не отправиться из-за сбоя)
    //
    //     CurrentStep = CurrentScenario.Steps.Find();
    //     // answerText = CurrentStep.Actions.Find(action => action.ActionId == inputText)
    // }
    
    

}
