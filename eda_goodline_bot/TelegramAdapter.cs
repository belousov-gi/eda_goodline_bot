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
    public TelegramAdapter(string token)
    {
        telegramClient = new HttpClient();
        this.token = token;
        ServerAddress = $"https://api.telegram.org/bot{token}";
    }

    public delegate void OnMessage(TelegramReceivedMessages messages);
    public event OnMessage OnMessages;


    public void Start()
    {
        OnMessages += MessageHandler;

        //TODO: вынести в какой-то пакет констант + таймаут для лонг пулинга

        //смотрим на сообщения, которые пришли до включения бота, чтобы определить с какого updateId начать
        // var updateId = FindLastUpdateId(ServerAddress);
        
        var updateId = 0;
        // serverAddress = $"https://api.telegram.org/bot{token}/getUpdates?offset={updateId}&timeout={timeout}";
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


    
    public async void MessageHandler(TelegramReceivedMessages messages)
    {
        // var answer = new Action();
        // string jsonString = JsonSerializer.Serialize(answer);
        // string serverAddress = $"https://api.telegram.org/bot{token}/sendMessage?chat_id={chatId}&text={$"{text}"}&reply_markup={jsonString}";
        // using var request = new HttpRequestMessage(HttpMethod.Get, serverAddress);
        // using var response = await telegramClient.SendAsync(request);
        //TODO: логирование ошибок навернуть + 
        await Task.Run(() =>
        {
            foreach (var messageInfo in messages.result)
            {
                var userId = messageInfo.message.from.id;
                var chatId = messageInfo.message.chat.id;
                var text = messageInfo.message.text;



                //TODO:убрать в отдельную функцию отправки
                // 
                string jsonString = null;
                var requestStr =
                    ServerAddress + $"/sendMessage?chat_id={chatId}&text={$"{text}&reply_markup={jsonString}"}";
                using var request = new HttpRequestMessage(HttpMethod.Get, requestStr);
                
                // Console.WriteLine($"Номер треда внутри хендлера ДО отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");

                using var response = telegramClient.Send(request);
                
                if ((int)response.StatusCode != 200)
                {
                    //TODO:залогировать ошибку отправки
                }

                // Console.WriteLine(
                //     $"Номер треда внутри хендлера ПОСЛЕ отправки запроса {Thread.GetCurrentProcessorId()} текст {text}");
            }
        });

    }


}
