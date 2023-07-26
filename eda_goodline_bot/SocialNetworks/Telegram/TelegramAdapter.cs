using eda_goodline_bot.Iterfaces;
using System.Net.Http;
using Telegram.Bot.Types.ReplyMarkups;
using System.Text.Json;
using System.Text.Json.Nodes;
using eda_goodline_bot.Models;


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
    
    public delegate void OnMessage(TelegramReceivedMessages messages);
    public event OnMessage OnMessages;
    

    public TelegramAdapter(string token, Scenario loadedScenario)
    {
        telegramClient = new HttpClient();
        this.token = token;
        ServerAddress = $"https://api.telegram.org/bot{token}";
        LoadedScenario = loadedScenario;
    }

    
    


    public void Start()
    {
        
        

        //TODO: вынести в какой-то пакет констант + таймаут для лонг пулинга

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
            ++updateId;

            messages = CheckNewMessages(serverAddress, updateId);
            resultLenght = messages.result.Length;
            
            if (resultLenght != 0)
            {
                Console.WriteLine($"Номер треда при триггере события {Thread.GetCurrentProcessorId()}");
                OnMessages?.Invoke(messages);
                updateId = messages.result[resultLenght - 1].update_id;  
            }
        }
    }


    private TelegramReceivedMessages CheckNewMessages(string serverAddress, int updateId)
    {
        const int timeout = 60;
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
        TelegramReceivedMessages? messages = CheckNewMessages(serverAddress, updateId);
        
        Console.WriteLine($"смотрим кол-во пропущенных сообщений {messages.result.Length}");
        int resultLenght = messages.result.Length;
        
        if (resultLenght != 0)
        {
            updateId = messages.result[resultLenght - 1].update_id;  
        }
        return updateId;
    }

    public void SendGeneralOrder()
    {
        throw new NotImplementedException();
    }



}
